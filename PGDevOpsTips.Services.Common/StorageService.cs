using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using PGDevOpsTips.Domain.Interfaces;
using PGDevOpsTips.Domain.Models;
using PGDevOpsTips.Services.Common.Models;
using System.Text;

namespace PGDevOpsTips.Services.Common
{
    //TODO: Move to workflow as you can't use storage sdk with WebAssembly blazor
    /// <inheritdoc/>
    public class StorageService : IStorageService
    {
        private readonly StorageConfiguration _options;
        private readonly BlobContainerClient _contentsBlobContainerClient;
        private readonly TableClient _contentsTableClient;
        private const string _contentsPartitionKey = "content";
        private readonly TableClient _shortcutsTableClient;
        private readonly BlobContainerClient _wallpapersBlobContainerClient;
        private const string _shortcutsPartitionKey = "shortcut";

        public StorageService(IOptions<StorageConfiguration> storageConfigurationOptions)
        {
            _options = storageConfigurationOptions.Value;

            _contentsTableClient = new TableClient(_options.ConnectionString, _options.ContentsTable);
            _contentsTableClient.CreateIfNotExists();

            _shortcutsTableClient = new TableClient(_options.ConnectionString, _options.ShortcutsTable);
            _shortcutsTableClient.CreateIfNotExists();

            _contentsBlobContainerClient = new BlobContainerClient(_options.ConnectionString, _options.ContentBlobsContainer);
            _contentsBlobContainerClient.CreateIfNotExists();

            _wallpapersBlobContainerClient = new BlobContainerClient(_options.ConnectionString, _options.WallpaperBlobsContainer);
            _wallpapersBlobContainerClient.CreateIfNotExists();
        }

        public async Task<string> UpsertBlob(string fileName, string fileContents)
        {
            // Get a reference to a blob
            var blobClient = _contentsBlobContainerClient.GetBlobClient(fileName);

            // Upload the file
            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(fileContents));
            await blobClient.UploadAsync(ms, overwrite: true);

            return blobClient.Uri.AbsoluteUri.ToString().ToLowerInvariant();
        }

        public async Task DeleteBlob(string fileName)
        {
            // Get a reference to a blob
            var blobClient = _contentsBlobContainerClient.GetBlobClient(fileName);

            // Delete blob
            await blobClient.DeleteIfExistsAsync(Azure.Storage.Blobs.Models.DeleteSnapshotsOption.IncludeSnapshots);
        }

        public async Task<string> GetBlobContent(string blobName)
        {
            var blobClient = _contentsBlobContainerClient.GetBlobClient(blobName);
            var blobDownloadInfo = await blobClient.DownloadAsync();
            string contents;
            using (var streamReader = new StreamReader(blobDownloadInfo.Value.Content))
            {
                contents = await streamReader.ReadToEndAsync();
            }
            if (contents.StartsWith("<p>."))
            {
                var contentsWithoutOpeningP = contents.Substring(4);
                contents = $"<p>{contentsWithoutOpeningP}";
            }
            return contents;
        }

        public async Task UpsertShortcut(Shortcut shortcut)
        {
            var entity = ConvertToTableEntity(shortcut, _shortcutsPartitionKey, shortcut.Key);
            await _shortcutsTableClient.UpsertEntityAsync(entity, TableUpdateMode.Replace);
        }

        public async Task DeleteShortcut(string shortcutKey)
        {
            await _shortcutsTableClient.DeleteEntityAsync(_shortcutsPartitionKey, shortcutKey);
        }

        public List<Shortcut> QueryShortcuts(string filter, int? take)
        {
            if (string.IsNullOrEmpty(filter))
            {
                filter = $"PartitionKey eq '{_shortcutsPartitionKey}'";
            }

            // Query shortcut entities
            var shortcutEntities = _shortcutsTableClient.Query<ShortcutEntity>(filter);

            // Cast ShortcutEntity to Shortcut
            var shortcuts = shortcutEntities.Select(x => new Shortcut() { Key = x.Key, Group = x.Group, Title = x.Title, Url = x.Url }).ToList();

            return shortcuts;
        }

        public async Task UpsertContent(Content content)
        {
            var entity = ConvertToTableEntity(content, _contentsPartitionKey, content.Key);
            await _contentsTableClient.UpsertEntityAsync(entity, TableUpdateMode.Replace);
        }

        public async Task DeleteContent(string contentKey)
        {
            await _contentsTableClient.DeleteEntityAsync(_contentsPartitionKey, contentKey);
        }

        public List<Content> QueryContents(string filter, bool includeDrafts, int? take)
        {
            if (string.IsNullOrEmpty(filter))
            {
                filter = $"PartitionKey eq '{_contentsPartitionKey}'";
            }

            // Query content entities
            var contentEntities = _contentsTableClient.Query<ContentEntity>(filter: filter);
            var contents = contentEntities.Select(x => new Content()
            {
                Key = x.Key,
                Title = x.Title,
                Author = x.Author,
                Description = x.Description,
                Image = x.Image,
                Thumbnail = x.Thumbnail,
                Published = x.Published,
                Categories = x.Categories,
                Type = x.Type,
                Status = x.Status,
                WebPath = x.WebPath,
                HtmlBlobPath = x.HtmlBlobPath,
                GitHubUrl = x.GitHubUrl,
                HtmlBlobFileName = x.HtmlBlobFileName
            }).ToList();

            //Sort by Published
            contents = contents.OrderByDescending(a => a.Published).ToList();

            // Remove drafts
            if (!includeDrafts)
            {
                contents = contents.Where(a => a.Status.ToLowerInvariant() == "published").ToList();
            }

            // Limit query if take is specified
            if (take != default)
            {
                var takeNotNullable = take ?? default(int);
                contents = contents.Take(takeNotNullable).ToList();
            }

            return contents;
        }

        public List<Content> GetContentsByProperty(string property, string value)
        {
            if (string.IsNullOrEmpty(property))
            {
                return null;
            }
            var filter = $"{property} eq '{value}'";
            var contents = QueryContents(filter, false, default);
            return contents;
        }

        public List<string> GetWallpaperUrls()
        {
            // Get wallpaper container SAS
            var containerSasUri = GetServiceSasUriForContainer(_wallpapersBlobContainerClient);

            // Get wallpaper blobs
            var blobs = _wallpapersBlobContainerClient.GetBlobs();

            // Build list of blob sas uris
            var blobSasUris = new List<string>();
            foreach (var blobItem in blobs)
            {
                var builder = new UriBuilder(containerSasUri);
                var segments = builder.Path.Split('/').ToList();
                segments.Add(blobItem.Name);
                builder.Path = String.Join('/', segments);
                blobSasUris.Add(builder.Uri.ToString());
            }
            return blobSasUris;
        }

        public string Heartbeat()
        {
            return $"ContentBlobsContainer is {_options.ContentBlobsContainer}";
        }

        private TableEntity ConvertToTableEntity(object o, string partitionKey, string rowkey)
        {
            var entity = new Azure.Data.Tables.TableEntity(partitionKey, rowkey);
            foreach (var prop in o.GetType().GetProperties())
            {
                var value = (prop.PropertyType.Name == nameof(DateTime)) ?
                    DateTime.SpecifyKind((DateTime)prop.GetValue(o), DateTimeKind.Utc) :
                    prop.GetValue(o);

                entity.Add(prop.Name, value);
            }
            return entity;
        }

        //TODO: Removed as can't use with WebAssembly Blazor
        private Uri GetServiceSasUriForContainer(BlobContainerClient containerClient, string storedPolicyName = null)
        {
            throw new NotImplementedException();
        }
    }
}

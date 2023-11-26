using PGDevOpsTips.Domain.Models;

namespace PGDevOpsTips.Domain.Interfaces
{
    /// <summary>
    /// Service for working with Azure Storage.
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// Insert or update a blob with new contents.
        /// </summary>
        /// <param name="fileName">The name of the blob i.e. test.md</param>
        /// <param name="fileContents">The new contents of the blob</param>
        /// <returns>Uri to the newly created or updated blob.</returns>
        public Task<string> UpsertBlob(string fileName, string fileContents);

        /// <summary>
        /// Delete a blob
        /// </summary>
        /// <param name="fileName">File name of the blob to delete i.e. Test.md</param>
        /// <returns>Task</returns>
        public Task DeleteBlob(string fileName);

        /// <summary>
        /// Helper method which gets the contents of a blob from storage as a string
        /// </summary>
        /// <param name="blobName">The file name of the blob in storage.</param>
        /// <returns>A string containingthe contents of a blob.</returns>
        public Task<string> GetBlobContent(string blobName);

        /// <summary>
        /// Insert or update an Shortcut entity.
        /// </summary>
        /// <param name="shortcut">Shortcut to upsert.</param>
        /// <returns>Task</returns>
        public Task UpsertShortcut(Shortcut shortcut);

        /// <summary>
        /// Delete an shortcut based on entity key
        /// </summary>
        /// <param name="shortcutKey">The Azure Storage Table entity key to delete.</param>
        /// <returns>Task.</returns>
        public Task DeleteShortcut(string shortcutKey);

        /// <summary>
        /// Gets a list of Shortcut
        /// </summary>
        /// <param name="filter">An OData filter string to be applied to the query. If ommited everything with the standard partition key will be used.</param>
        /// <param name="take">The number of Shortcuts to return. Defaults to 5.</param>
        /// <returns>List of shortcuts</returns>
        public List<Shortcut> QueryShortcuts(string filter, int? take);

        /// <summary>
        /// Insert or update Content entity.
        /// </summary>
        /// <param name="content">Content to upsert.</param>
        /// <returns>Task</returns>
        public Task UpsertContent(Content content);

        /// <summary>
        /// Delete content based on entity key
        /// </summary>
        /// <param name="contentKey">The Azure Storage Table entity key to delete.</param>
        /// <returns>Task.</returns>
        public Task DeleteContent(string contentKey);

        /// <summary>
        /// Gets a list of Contents
        /// </summary>
        /// <param name="filter">An OData filter string to be applied to the query. If ommited everything with the standard partition key will be used.</param>
        /// <param name="includeDrafts">If true only content where Status=Published. If false, everything is returned.</param>
        /// <param name="take">The number of contents to return. Sorted by most recent base don Published. Defaults to 5.</param>
        /// <returns>List of contents</returns>
        public List<Content> QueryContents(string filter, bool includeDrafts, int? take);

        /// <summary>
        /// Helper method which constructs an OData query and gets a list of Contents which match
        /// </summary>
        /// <param name="property">The property (key) to match the ContentEntity on.</param>
        /// <param name="property">The value to match the ContentEntity on.</param>
        /// <returns>Content</returns>
        public List<Content> GetContentsByProperty(string property, string value);

        /// <summary>
        /// Helper method which constructs an OData query and gets Content which match
        /// </summary>
        /// <param name="type">The Content Type to match the ContentEntity on.</param>
        /// <param name="property">The property (key) to match the ContentEntity on.</param>
        /// <param name="value">The value to match the ContentEntity on.</param>
        /// <param name="includeDrafts">If true only content where Status=Published. If false, everything is returned.</param>
        /// <returns></returns>
        Content GetContentByProperty(string type, string property, string value, bool includeDrafts);

        /// <summary>
        /// Returns list of urls for each blob in wallpapers container
        /// </summary>
        /// <returns>List of wallpaper blob urls</returns>
        public List<string> GetWallpaperUrls();

        /// <summary>
        /// Function to use to test avaliability of service
        /// </summary>
        /// <returns></returns>
        public string Heartbeat();
    }
}

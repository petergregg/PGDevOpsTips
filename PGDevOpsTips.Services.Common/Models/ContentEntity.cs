using Azure;
using Azure.Data.Tables;
using PGDevOpsTips.Domain.Interfaces;

namespace PGDevOpsTips.Services.Common.Models
{
    /// <inheritdoc/>
	public class ContentEntity : ITableEntity, IContent
    {
        public string Key { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string Author { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Image { get; set; } = default!;
        public string Thumbnail { get; set; } = default!;
        public DateTime Published { get; set; } = default!;
        public string Categories { get; set; } = default!;
        public string Type { get; set; } = default!;
        public string Status { get; set; } = default!;
        public string WebPath { get; set; } = default!;
        public string HtmlBlobPath { get; set; } = default!;
        public string GitHubUrl { get; set; } = default!;
        public string HtmlBlobFileName { get; set; } = default!;
        public string PartitionKey { get; set; } = default!;
        public string RowKey { get; set; } = default!;
        public DateTimeOffset? Timestamp { get; set; } = DateTimeOffset.UtcNow!;
        public ETag ETag { get; set; } = default!;
    }
}

using PGDevOpsTips.Domain.Interfaces;

namespace PGDevOpsTips.Domain.Models
{
    public class Content : IContent
    {
        public string Key { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Thumbnail { get; set; }
        public DateTime Published { get; set; }
        public string Categories { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string WebPath { get; set; }
        public string HtmlBlobPath { get; set; }
        public string GitHubUrl { get; set; }
        public string HtmlBlobFileName { get; set; }
    }
}

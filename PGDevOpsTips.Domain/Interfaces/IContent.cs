namespace PGDevOpsTips.Domain.Interfaces
{
    /// <summary>
    /// An interface to define Content.
    /// </summary>
    public interface IContent
    {
        /// <summary>
        /// The unique ID and Azure Storage Table entity key. Typically a base64 encoded version of the Github path, for example blogs/Test.md will result in a key of YmxvZ3MvdGVzdC5tZA==
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The title of the content
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The author of the content
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// The description of the content
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// An absolute Url to an image to be used in the content itself
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// An absolute Url to a thumbnail image to be used in when listing the content amongst others
        /// </summary>
        public string Thumbnail { get; set; }

        /// <summary>
        /// The type of content, examples "article", "resource"
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The date the content was published
        /// </summary>
        public DateTime Published { get; set; }

        /// <summary>
        /// A comma delimited list of tags or categories for the content
        /// </summary>
        public string Categories { get; set; }

        /// <summary>
        /// The status of the content, Typically either "draft" or "published"
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// A web friendly path for the content based on Title which will be used to form the web url. If the Title is "Coding for Dummies", the path will be "coding-for-dummies"
        /// </summary>
        public string WebPath { get; set; }

        /// <summary>
        /// The absolute uri to the related html blob in Azure Storage. Does not require key or sas token.
        /// </summary>
        public string HtmlBlobPath { get; set; }

        /// <summary>
        /// The absolute uri to the related markdown file on GitHub.
        /// </summary>
        public string GitHubUrl { get; set; }

        /// <summary>
        /// The file name of the related html blob in Azure Storage.
        /// </summary>
        public string HtmlBlobFileName { get; set; }
    }
}

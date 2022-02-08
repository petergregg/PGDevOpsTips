using System;
using System.Collections.Generic;

namespace PGDevOpsTips.Web.Models
{
    /// <summary>
    /// Represents Content. Derived from the GithubContent and ContentYAMLHeader stored in a markdown file in Github.
    /// </summary>
    public class Content
    {
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
        /// An absolute Url to a thumbnail image to be used when listing the content amogst others
        /// </summary>
        public string Thumbnail { get; set; }

        /// <summary>
        /// The type of content, examples "article", "resource"
        /// </summary>
        public string Type { get; internal set; }

        /// <summary>
        /// The date the content was published
        /// </summary>
        public DateTime Published { get; set; }

        /// <summary>
        /// A comma delimited collection of tags or categories for the content
        /// </summary>
        public IEnumerable<string> Categories { get; set; }

        /// <summary>
        /// The full Uri to get information (GithubContent) about a given file from the Github content Api 
        /// or the plain (decoded) content of the file converted to html.
        /// </summary>
        public string Html { get; set; }

        /// <summary>
        /// The status of the content, typically either "draft" or "published"
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// A web friendly path for the content based on Title which will be used to form the web url. If the Title is "Coding for Dummies", the path will be "coding-for-dummies"
        /// </summary>
        public string WebPath { get; set; }

        /// <summary>
        /// The path to a file
        /// </summary>
        public string GitHubPath { get; set; }       
    }
}

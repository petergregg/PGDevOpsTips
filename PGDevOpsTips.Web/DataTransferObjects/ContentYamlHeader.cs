using System;
using System.Collections.Generic;

namespace PGDevOpsTips.Web.DataTransferObjects
{
    /// <summary>
    /// Class used to deserialise the Yaml Header contained in the Markdown for a given files content.
    /// </summary>
    public class ContentYamlHeader
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
        /// An absolute Url to a thumbnail image to be used when listing the content amongst others
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
        /// A comma delimited collection of tags or categories for the content
        /// </summary>
        public List<string> Categories { get; set; }

        /// <summary>
        /// The status of the content, typically either "draft" or "published"
        /// </summary>
        public string Status { get; set; }
    }
}

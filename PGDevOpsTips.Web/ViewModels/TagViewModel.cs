using PGDevOpsTips.Web.Models;
using System.Collections.Generic;

namespace PGDevOpsTips.Web.ViewModels
{
    /// <summary>
    /// Class used for representing Tagged Articles.
    /// </summary>
    public class TagViewModel
    {
        /// <summary>
        /// The tag associated with content
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// A Collection of contents, typically articles, resources
        /// </summary>
        public List<Content> Contents { get; set; }
    }
}

using PGDevOpsTips.Domain.Models;
using System.Collections.Generic;

namespace PGDevOpsTips.Web.ViewModels
{
    /// <summary>
    /// Class used for representing Resources.
    /// </summary>
    public class ResourcesViewModel
    {
        /// <summary>
        /// A Collection of articles
        /// </summary>
        public List<Content> Resources { get; set; }
    }
}

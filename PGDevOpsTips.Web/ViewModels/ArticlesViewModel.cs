using PGDevOpsTips.Domain.Models;
using System.Collections.Generic;

namespace PGDevOpsTips.Web.ViewModels
{
    /// <summary>
    /// Class used for representing Articles.
    /// </summary>
    public class ArticlesViewModel
    {
        /// <summary>
        /// A Collection of articles
        /// </summary>
        public List<Content> Articles { get; set; }
    }
}

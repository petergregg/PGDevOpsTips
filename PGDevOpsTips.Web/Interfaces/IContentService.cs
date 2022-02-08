using PGDevOpsTips.Web.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PGDevOpsTips.Web.Interfaces
{
    /// <summary>
    /// Service for working with Content.
    /// </summary>
    public interface IContentService
    {
        /// <summary>
        /// Gets a collection of contents from GitHub
        /// </summary>
        /// <param name="requestUri">A string that represents the request System.Uri (https://api.github.com/repos/petergregg/content/contents/Blog/Articles).</param>
        /// <param name="cancellationToken">The CancellationToken.</param>
        /// <returns>Returns a collection of Content objects.</returns>
        Task<List<Content>> GetContents(string requestUri, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a collection of contents from GitHub by metadata tag
        /// </summary>
        /// <param name="requestUri">A string that represents the request System.Uri.</param>
        /// <param name="tag">A string that represents the contents tag.</param>
        /// <param name="cancellationToken">The CancellationToken.</param>
        /// <returns>Returns a collection of Content objects.</returns>
        Task<List<Content>> GetContentsByTag(string requestUri, string tag, CancellationToken cancellationToken);

        /// <summary>
        /// Gets content from GitHub
        /// </summary>
        /// <param name="requestUri">A string that represents the request System.Uri.</param>
        /// <param name="contentWebPath">A string that represents a web friendly path for the content.</param>
        /// <param name="cancellationToken">The CancellationToken.</param>
        /// <returns>Returns a Content object.</returns>
        Task<Content> GetContent(string requestUri, string contentWebPath, CancellationToken cancellationToken);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using PGDevOpsTips.Domain.Models;

namespace PGDevOpsTips.Web.Interfaces
{
    public interface IRestContentService
    {
        Task<List<Content>> GetContentsList(string requestUri, CancellationToken cancellationToken);

        Task<string> GetBlobContent(string requestUri, CancellationToken cancellationToken);

        Task<Content> GetContent(string requestUri, CancellationToken cancellationToken);
    }
}

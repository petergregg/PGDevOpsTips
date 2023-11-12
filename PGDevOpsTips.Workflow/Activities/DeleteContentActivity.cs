using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using PGDevOpsTips.Domain.Interfaces;
using System.Threading.Tasks;

namespace PGDevOpsTips.Workflow.Activities
{
    public sealed class DeleteContentActivity
    {
        private readonly IStorageService _storageService;

        public DeleteContentActivity(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [FunctionName(nameof(DeleteContentActivity))]
        public async Task RunDeleteContentActivity([ActivityTrigger] string contentKey, ILogger log)
        {
            log.LogInformation("Delete content {contentKey}.", contentKey);
            await _storageService.DeleteContent(contentKey);
        }
    }
}

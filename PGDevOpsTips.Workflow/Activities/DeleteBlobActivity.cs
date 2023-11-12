using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using PGDevOpsTips.Domain.Interfaces;
using System.Threading.Tasks;

namespace PGDevOpsTips.Workflow.Activities
{
    public sealed class DeleteBlobActivity
    {
        private readonly IStorageService _storageService;

        public DeleteBlobActivity(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [FunctionName(nameof(DeleteBlobActivity))]
        public async Task RunDeleteBlobActivity([ActivityTrigger] string htmlBlobFileName, ILogger log)
        {
            log.LogInformation("Delete blob {htmlBlobFileName}.", htmlBlobFileName);
            await _storageService.DeleteBlob(htmlBlobFileName);
        }
    }
}

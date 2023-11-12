using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using PGDevOpsTips.Domain.Interfaces;
using PGDevOpsTips.Domain.Models;
using System.Threading.Tasks;

namespace PGDevOpsTips.Workflow.Activities
{
    public sealed class UpsertContentActivity
    {
        private readonly IStorageService _storageService;

        public UpsertContentActivity(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [FunctionName(nameof(UpsertContentActivity))]
        public async Task RunUpsertContentActivity([ActivityTrigger] Content content, ILogger log)
        {
            log.LogInformation("Upsert content {content}.", content);
            await _storageService.UpsertContent(content);
        }
    }
}

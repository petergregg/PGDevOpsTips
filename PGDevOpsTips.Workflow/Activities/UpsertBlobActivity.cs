using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using PGDevOpsTips.Domain.Interfaces;
using PGDevOpsTips.Workflow.Models;
using System.Threading.Tasks;

namespace PGDevOpsTips.Workflow.Activities
{
    public sealed class UpsertBlobActivity
    {
        private readonly IStorageService _storageService;

        public UpsertBlobActivity(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [FunctionName(nameof(UpsertBlobActivity))]
        public async Task<string> RunUpsertBlobActivity([ActivityTrigger] ContentContext contentContext, ILogger log)
        {
            log.LogInformation("Upsert blob {contentContext.HtmlBlobFileName}.", contentContext.HtmlBlobFileName);
            return await _storageService.UpsertBlob(contentContext.HtmlBlobFileName, contentContext.PlainHtmlContents);
        }
    }
}

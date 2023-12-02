using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PGDevOpsTips.Domain.Interfaces;

namespace PGDevOpsTips.Workflow.API
{
    public sealed class GetBlobContent
    {
        private readonly IStorageService _storageService;

        public GetBlobContent(IStorageService storageService)
        {
            _storageService = storageService;
        }

        //TODO: Add not found

        [FunctionName("GetBlobContent")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "blobcontent")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string blobName = req.Query["blobname"];

            string responseMessage = string.IsNullOrEmpty(blobName) 
                ? "Pass a blobname in the query string"
                : await _storageService.GetBlobContent(blobName);

            return new OkObjectResult(responseMessage);
        }
    }
}

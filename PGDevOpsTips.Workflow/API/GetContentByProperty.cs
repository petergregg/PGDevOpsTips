using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PGDevOpsTips.Domain.Interfaces;
using System;

namespace PGDevOpsTips.Workflow.API
{
    public sealed class GetContentByProperty
    {
        private readonly IStorageService _storageService;

        public GetContentByProperty(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [FunctionName("GetContentByProperty")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "contents/content")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string type = req.Query["type"];
            string property = req.Query["property"];
            string value = req.Query["value"];
            string drafts = req.Query["drafts"];
            var includeDrafts = Convert.ToBoolean(drafts);

            if (type == null || property == null || value == null)
            {
                return new OkObjectResult("Pass a type, property or value into the query string");
            }

            var content = _storageService.GetContentByProperty(type, property, value, includeDrafts);

            if (content == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(content);
        }
    }
}

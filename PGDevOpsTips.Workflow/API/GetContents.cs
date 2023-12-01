using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PGDevOpsTips.Domain.Interfaces;
using System.Collections.Generic;
using PGDevOpsTips.Domain.Models;

namespace PGDevOpsTips.Workflow.API
{
    public sealed class GetContents
    {
        private readonly IStorageService _storageService;

        public GetContents(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [FunctionName("GetContents")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "contents")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string type = req.Query["type"];

            var filter = default(string);
            if (type != null)
            {
                filter = $"Type eq '{type}'";
            }

            string drafts = req.Query["drafts"];
            var includeDrafts = Convert.ToBoolean(drafts);

            List<Content> contents = new List<Content>();
            contents = _storageService.QueryContents(filter, includeDrafts, default);

            return new OkObjectResult(contents);
        }
    }
}

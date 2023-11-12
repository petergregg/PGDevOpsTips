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
    public sealed class ContentAPI
    {
        private readonly IStorageService _storageService;

        public ContentAPI(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [FunctionName("ContentAPI")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "content/{type:alpha}/{includeDrafts:bool}")] HttpRequest req,
            string type,
            Boolean includeDrafts,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            List<Content> contents = new List<Content>();

            var filter = $"Type eq '{type}'";

            contents = _storageService.QueryContents(filter, includeDrafts, default);

            return new OkObjectResult(contents);
        }
    }
}

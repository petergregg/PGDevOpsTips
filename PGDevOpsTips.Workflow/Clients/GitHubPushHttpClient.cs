using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using PGDevOpsTips.Workflow.DTOs;
using PGDevOpsTips.Workflow.Orchestrations;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PGDevOpsTips.Workflow.Clients
{
    public static class GitHubPushHttpClient
    {
        [FunctionName("GitHubPushHttpClient_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {

            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await req.Content.ReadAsStringAsync();

            GitHubPush gitHubPushPayload;

            try
            {
                gitHubPushPayload = JsonSerializer.Deserialize<GitHubPush>(requestBody);
                log.LogInformation("Received Github webhook for commit {CommitId}", gitHubPushPayload.Commits[0].Id);
            }
            catch (NullReferenceException nullException)
            {

                string exceptionMessage = "GitHub Push Payload was not in expected format";
                log.LogError(nullException, exceptionMessage);

                var response = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                //response.WriteString(exceptionMessage);
                return response;
            }

            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync(nameof(MainOrchestration), gitHubPushPayload);

            log.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
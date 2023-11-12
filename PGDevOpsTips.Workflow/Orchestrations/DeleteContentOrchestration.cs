using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using PGDevOpsTips.Workflow.Activities;
using PGDevOpsTips.Workflow.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PGDevOpsTips.Workflow.Orchestrations
{
    public class DeleteContentOrchestration
    {
        [FunctionName(nameof(DeleteContentOrchestration))]
        public async Task<List<string>> RunDeleteContentOrchestration([OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            // Get input payload
            ContentContext contentContext = context.GetInput<ContentContext>();

            // Delete blob
            await context.CallActivityAsync(nameof(DeleteBlobActivity), contentContext.HtmlBlobFileName);

            // Delete content
            await context.CallActivityAsync(nameof(DeleteContentActivity), contentContext.Content.Key);

            List<string> outputs = new()
            {
                $"Deleted {contentContext.GithubContentApiUri}"
            };

            return outputs;
        }
    }
}

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using PGDevOpsTips.Domain.Models;
using PGDevOpsTips.Workflow.DTOs;
using PGDevOpsTips.Workflow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGDevOpsTips.Workflow.Orchestrations
{
    public class MainOrchestration
    {
        [FunctionName(nameof(MainOrchestration))]
        public static async Task<List<string>> RunOrchestrator(
        [OrchestrationTrigger] IDurableOrchestrationContext context)
        {

            // Get input payload
            GitHubPush input = context.GetInput<GitHubPush>();

            var outputs = new List<string>();

            foreach (Commit commit in input.Commits)
            {
                // Items added in commit
                outputs.AddRange(await CallSubOrchestration(context, commit.Added, nameof(AddModifyContentOrchestration), commit.Author.Username, input.Repository.Name));

                // Items modified in commit
                outputs.AddRange(await CallSubOrchestration(context, commit.Modified, nameof(AddModifyContentOrchestration), commit.Author.Username, input.Repository.Name));

                // Items deleted in commit
                outputs.AddRange(await CallSubOrchestration(context, commit.Removed, nameof(DeleteContentOrchestration), commit.Author.Username, input.Repository.Name));
            }

            return outputs;
        }


        private static async Task<List<string>> CallSubOrchestration([OrchestrationTrigger] IDurableOrchestrationContext context, List<string> items, string subOrchestration, string author, string repo)
        {
            List<string> outputs = new();

            // Filter the items to only include markdown files in the /blogs/ folder
            List<string> filteredItems = items
                .Where(i => i.EndsWith(".md", StringComparison.InvariantCultureIgnoreCase))
                .Where(i => i.StartsWith("blog/", StringComparison.InvariantCultureIgnoreCase))
                .ToList();

            foreach (string item in filteredItems)
            {
                // Prepare the content key (base64 encoded version of the gihub path .. i.e blogs/Test.md)
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(item.ToLowerInvariant());
                string contentKey = Convert.ToBase64String(plainTextBytes);
                Content content = new() { Key = contentKey };

                // Prepare content context
                ContentContext contentContext = new()
                {
                    GithubContentApiUri = new Uri($"https://api.github.com/repos/{author}/{repo}/contents/{item}"),
                    HtmlBlobFileName = $"{contentKey}.html",
                    Content = content,
                };

                // Call sub-orchestration passing content contents
                List<string> subOrchestrationOutput = await context.CallSubOrchestratorAsync<List<string>>(subOrchestration, contentContext);
                outputs.AddRange(subOrchestrationOutput);
            }

            return outputs;
        }
    }
}

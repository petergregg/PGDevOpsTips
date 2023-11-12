using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using PGDevOpsTips.Domain.Models;
using PGDevOpsTips.Workflow.Activities;
using PGDevOpsTips.Workflow.DTOs;
using PGDevOpsTips.Workflow.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PGDevOpsTips.Workflow.Orchestrations
{
    public class AddModifyContentOrchestration
    {

        [FunctionName(nameof(AddModifyContentOrchestration))]
        public static async Task<List<string>> RunAddModifyContentOrchestration([OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            // Get input payload
            ContentContext contentContext = context.GetInput<ContentContext>();

            // Get Github content
            contentContext.GithubContent = await context.CallActivityAsync<GithubContent>(nameof(GetGithubContentActivity), contentContext.GithubContentApiUri);

            // Decode the Base64 contents
            contentContext.PlainContents = (contentContext.GithubContent.Encoding == "base64") ?
                Encoding.UTF8.GetString(Convert.FromBase64String(contentContext.GithubContent.Content)) :
                contentContext.GithubContent.Content;

            // Convert plain Markdown to Html
            contentContext.PlainHtmlContents = await context.CallActivityAsync<string>(nameof(MarkdownToHtmlActivity), contentContext.PlainContents);

            // Upsert html blob to storage
            contentContext.HtmlBlobStorageUri = await context.CallActivityAsync<string>(nameof(UpsertBlobActivity), contentContext); //Requires HtmlBlobFileName and PlainContents from ContentContext

            // Convert Yaml to a Content object
            contentContext.Content = await context.CallActivityAsync<Content>(nameof(YamlToMarkdownActivity), contentContext); // Requires PlainContents, HtmlBlobStorageUri, Content from ContentContext

            // Add other properties to Content
            contentContext.Content.GitHubUrl = contentContext.GithubContent.HtmlUrl;
            contentContext.Content.HtmlBlobFileName = contentContext.HtmlBlobFileName;
            var pathSb = new StringBuilder(string.Join("-", contentContext.Content.Title.Split(Path.GetInvalidFileNameChars())));
            pathSb.Replace(" ", "-");
            contentContext.Content.WebPath = pathSb.ToString().ToLowerInvariant();

            // Upsert Content in table storage
            await context.CallActivityAsync(nameof(UpsertContentActivity), contentContext.Content);

            List<string> outputs = new()
            {
                $"Added/modified {contentContext.GithubContentApiUri}"
            };

            return outputs;
        }
    }
}

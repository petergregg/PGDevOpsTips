using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using PGDevOpsTips.Workflow.Interfaces;

namespace PGDevOpsTips.Workflow.Activities
{
    public sealed class MarkdownToHtmlActivity
    {
        private readonly IMarkdownService _markdownService;

        public MarkdownToHtmlActivity(IMarkdownService markdownService)
        {
            _markdownService = markdownService;
        }

        [FunctionName(nameof(MarkdownToHtmlActivity))]
        public string RunMarkdownToHtmlActivity([ActivityTrigger] string plainContents, ILogger log)
        {
            log.LogInformation("Convert markdown to html {plainContents}.", plainContents);
            return _markdownService.MarkdownToHtml(plainContents);
        }
    }
}

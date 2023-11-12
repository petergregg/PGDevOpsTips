using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using PGDevOpsTips.Domain.Models;
using PGDevOpsTips.Workflow.Interfaces;
using PGDevOpsTips.Workflow.Models;

namespace PGDevOpsTips.Workflow.Activities
{
    public sealed class YamlToMarkdownActivity
    {
        private readonly IYamlService _yamlService;

        public YamlToMarkdownActivity(IYamlService yamlService)
        {
            _yamlService = yamlService;
        }

        [FunctionName(nameof(YamlToMarkdownActivity))]
        public Content RunYamlToMarkdownActivity([ActivityTrigger] ContentContext contentContext, ILogger log)
        {
            // Convert yaml to content
            log.LogInformation("Convert yaml to content {contentContext.PlainContents}.", contentContext.PlainContents);
            return _yamlService.YamlToContent(contentContext.PlainContents, contentContext.Content);
        }
    }
}

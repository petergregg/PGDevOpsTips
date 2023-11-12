using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using PGDevOpsTips.Workflow.DTOs;
using PGDevOpsTips.Workflow.Interfaces;
using System.Threading.Tasks;

namespace PGDevOpsTips.Workflow.Activities
{
    public sealed class GetGithubContentActivity
    {
        private readonly IGithubService _githubService;

        public GetGithubContentActivity(IGithubService githubService)
        {
            _githubService = githubService;
        }

        [FunctionName(nameof(GetGithubContentActivity))]
        public async Task<GithubContent> RunGetGithubContentActivity([ActivityTrigger] string githubContentApiUri, ILogger log)
        {
            log.LogInformation("Get GitHub content {githubContentApiUri}.", githubContentApiUri);
            return await _githubService.GetGithubContent(githubContentApiUri);
        }
    }
}

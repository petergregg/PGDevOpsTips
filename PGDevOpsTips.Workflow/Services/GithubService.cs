using PGDevOpsTips.Workflow.DTOs;
using PGDevOpsTips.Workflow.Interfaces;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PGDevOpsTips.Workflow.Services
{
    /// <inheritdoc/>
    public class GithubService : IGithubService
    {
        private readonly IHttpClientFactory _clientFactory;

        public GithubService(IHttpClientFactory httpClientFactory)
        {
            _clientFactory = httpClientFactory;
        }

        public async Task<GithubContent> GetGithubContent(string fileApiUrl)
        {
            // Make request to Github
            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri(fileApiUrl);
            client.DefaultRequestHeaders.Add("User-Agent", "PGDevOpsTips - GetFileContentsActivity");
            var response = await client.GetAsync(fileApiUrl);
            response.EnsureSuccessStatusCode();

            // Read and decode contents
            var rawContents = await response.Content.ReadAsStringAsync();
            var objContents = JsonSerializer.Deserialize<GithubContent>(rawContents);

            // Return
            return objContents;
        }
    }
}

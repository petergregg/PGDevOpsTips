using PGDevOpsTips.Web.Interfaces;
using PGDevOpsTips.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using PGDevOpsTips.Web.DataTransferObjects;
using System.Text.Json;
using System.Threading;

namespace PGDevOpsTips.Web.Services
{
    /// <inheritdoc/>
    public class ContentService : IContentService
    {
        protected readonly HttpClient _httpClient;
        protected readonly IYamlService _yamlService;
        protected readonly IMarkdownService _markdownService;

        public ContentService(HttpClient httpClient, IYamlService yamlService, IMarkdownService markdownService)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _yamlService = yamlService;
            _markdownService = markdownService;
        }

        public async Task<Content> GetContent(string requestUri, string contentWebPath, CancellationToken cancellationToken)
        {
            var contents = await GetContents(requestUri, cancellationToken).ConfigureAwait(false);
            var content = contents.FirstOrDefault(o => o.WebPath.ToLower() == contentWebPath.ToLower());

            if (content == null)
                return content;

            content.Html = _markdownService.MarkdownToHtml(await GetPlainContents(content.Html, cancellationToken).ConfigureAwait(false));

            return content;
        }

        /// <summary>
        /// Converts base64 to a html string object.
        /// </summary>
        /// <param name="requestUri">A string that represents the request System.Uri.</param>
        /// <param name="cancellationToken">The CancellationToken.</param>
        /// <returns>Returns content as a html string</returns>
        private async Task<string> GetPlainContents(string requestUri, CancellationToken cancellationToken)
        {
            var content = await GetGithubContent(requestUri, cancellationToken).ConfigureAwait(false);
            var plainFileContents = (content.Encoding == "base64") ?
                    Encoding.UTF8.GetString(Convert.FromBase64String(content.Content)) :
                    content.Content;
            return plainFileContents;
        }

        public async Task<List<Content>> GetContents(string requestUri, CancellationToken cancellationToken)
        {
            var contents = new List<Content>();

            var request = CreateGetRequest(requestUri);
            using (var result = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false))
            {
                using var contentStream = await result.Content.ReadAsStreamAsync(cancellationToken);
                var githubContents = await JsonSerializer.DeserializeAsync<List<GithubContent>>(contentStream, DefaultJsonSerializerOptions.Options, cancellationToken);

                foreach (var githubContent in githubContents)
                {
                    var content = new Content();

                    content = _yamlService.YamlToContent(await GetPlainContents(githubContent.Url, cancellationToken).ConfigureAwait(false), content);
                    if (content.Title == null || content.Status == null || content.Type == null) //|| articleContent1.Status == ContentStatus.Draft)
                    {
                        continue;
                    }

                    // Add GitHub api endpoint for content
                    content.Html = githubContent.Url;
                    content.GitHubPath = githubContent.Path;

                    contents.Add(content);
                }
            }

            return contents;
        }

        /// <summary>
        /// Gets content for a Uniform Resource Identifier (URI) of a GitHub resource.
        /// </summary>
        /// <param name="requestUri">A string that represents the request System.Uri.</param>
        /// <param name="cancellationToken">The cancellationToken.</param>
        /// <returns>Returns a GithubContent object.</returns>
        private async Task<GithubContent> GetGithubContent(string requestUri, CancellationToken cancellationToken)
        {
            var request = CreateGetRequest(requestUri);
            using var result = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
            using var contentStream = await result.Content.ReadAsStreamAsync(cancellationToken);
            return await JsonSerializer.DeserializeAsync<GithubContent>(contentStream, DefaultJsonSerializerOptions.Options, cancellationToken);
        }

        /// <summary>
        /// Creates a Get request for a Uniform Resource Identifier (URI) of an internet resource.
        /// </summary>
        /// <param name="requestUri">A string that represents the request System.Uri.</param>
        /// <returns>Returns a HttpRequestMessage.</returns>
        private static HttpRequestMessage CreateGetRequest(string requestUri)
        {
            return new HttpRequestMessage(HttpMethod.Get, requestUri);
        }

        public async Task<List<Content>> GetContentsByTag(string requestUri, string tag, CancellationToken cancellationToken)
        {
            // all published contents
            var contentParents = await GetParentContents(requestUri, cancellationToken).ConfigureAwait(false);

            var contentsInTag = new List<Content>();

            foreach (var contentParent in contentParents.Where(x => !x.GitHubPath.Contains("Images")))
            {
                var allContents = await GetContents(contentParent.GitHubPath, cancellationToken).ConfigureAwait(false);

                List<Content> contents = allContents.Where(o => o.Status == ContentStatus.Published).ToList();

                // contents for tag
                foreach (var content in contents)
                {
                    foreach (var cat in content.Categories)
                    {
                        if (cat.ToLowerInvariant() == tag.ToLowerInvariant())
                            contentsInTag.Add(content);
                    }
                }
            }
            return contentsInTag;
        }

        /// <summary>
        /// Gets content parent
        /// </summary>
        /// <param name="requestUri">A string that represents the request System.Uri.</param>
        /// <param name="cancellationToken">The cancellationToken.</param>
        /// <returns></returns>
        private async Task<List<Content>> GetParentContents(string requestUri, CancellationToken cancellationToken)
        {
            var contents = new List<Content>();

            var request = CreateGetRequest(requestUri);
            using (var result = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false))
            {
                using var contentStream = await result.Content.ReadAsStreamAsync(cancellationToken);
                var githubContents = await JsonSerializer.DeserializeAsync<List<GithubContent>>(contentStream, DefaultJsonSerializerOptions.Options, cancellationToken);

                foreach (var githubContent in githubContents)
                {
                    var content = new Content
                    {
                        GitHubPath = githubContent.Url
                    };

                    contents.Add(content);
                }
            }

            return contents;
        }
    }
}

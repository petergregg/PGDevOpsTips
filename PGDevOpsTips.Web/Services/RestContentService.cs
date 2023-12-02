using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using PGDevOpsTips.Web.Interfaces;
using System;
using System.Text.Json;
using PGDevOpsTips.Domain.Models;

namespace PGDevOpsTips.Web.Services
{
    public class RestContentService : IRestContentService
    {
        protected readonly HttpClient _httpClient;

        public RestContentService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<List<Content>> GetContentsList(string requestUri, CancellationToken cancellationToken)
        {
            var contents = new List<Content>();

            var request = CreateGetRequest(requestUri);

            using (var result = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                using var contentStream = await result.Content.ReadAsStreamAsync(cancellationToken);
                contents = await JsonSerializer.DeserializeAsync<List<Content>>(contentStream, DefaultJsonSerializerOptions.Options, cancellationToken);
            }

            return contents;
        }

        public async Task<Content> GetContent(string requestUri, CancellationToken cancellationToken)
        {
            var content = new Content();

            var request = CreateGetRequest(requestUri);

            using (var result = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                using var contentStream = await result.Content.ReadAsStreamAsync(cancellationToken);
                content = await JsonSerializer.DeserializeAsync<Content>(contentStream, DefaultJsonSerializerOptions.Options, cancellationToken);
            }

            return content;
        }

        public async Task<string> GetBlobContent(string requestUri, CancellationToken cancellationToken)
        {
            string contents;

            var request = CreateGetRequest(requestUri);

            using (var result = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                contents = await result.Content.ReadAsStringAsync(cancellationToken);
            }

            return contents;
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
    }
}

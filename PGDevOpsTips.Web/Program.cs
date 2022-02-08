using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PGDevOpsTips.Web.Interfaces;
using PGDevOpsTips.Web.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PGDevOpsTips.Web
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddSingleton<IMarkdownService, MarkdownService>();
            builder.Services.AddSingleton<IYamlService, YamlService>();

            builder.Services.AddHttpClient<ContentService>("github", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["GitHubAPI"]);
                // Github API versioning
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
                // Github requires a user-agent
                client.DefaultRequestHeaders.Add("User-Agent", "PGDevOpsTips.Web");
            });

            await builder.Build().RunAsync();
        }
    }
}

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
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

            ConfigureServices(builder.Services, builder.HostEnvironment.BaseAddress, builder.Configuration);

            await builder.Build().RunAsync();
        }

        private static void ConfigureServices(IServiceCollection services, string baseAddress, IConfiguration configuration)
        {
            services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseAddress) });

            services.AddSingleton<IMarkdownService, MarkdownService>();
            services.AddSingleton<IYamlService, YamlService>();

            services.AddHttpClient<ContentService>("github", client =>
            {
                client.BaseAddress = new Uri(configuration["GitHubAPI"]);
                // Github API versioning
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
                // Github requires a user-agent
                client.DefaultRequestHeaders.Add("User-Agent", "PGDevOpsTips.Web");
            });
        }
    }
}

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PGDevOpsTips.Domain.Interfaces;
using PGDevOpsTips.Domain.Models;
using PGDevOpsTips.Services.Common;
using PGDevOpsTips.Workflow.Interfaces;
using PGDevOpsTips.Workflow.Services;

[assembly: FunctionsStartup(typeof(PGDevOpsTips.Workflow.Startup))]

namespace PGDevOpsTips.Workflow
{

    public class Startup : FunctionsStartup
    {

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();

            builder.Services.AddSingleton<IStorageService, StorageService>();

            builder.Services.AddSingleton<IGithubService, GithubService>();

            builder.Services.AddSingleton<IMarkdownService, MarkdownService>();

            builder.Services.AddSingleton<IYamlService, YamlService>();

            builder.Services.AddOptions<StorageConfiguration>()
               .Configure<IConfiguration>((settings, configuration) =>
               {
                   configuration.GetSection(nameof(StorageConfiguration)).Bind(settings);
               });
        }
    }
}

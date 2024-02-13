using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            builder.RootComponents.Add<HeadOutlet>("head::after");

            ConfigureServices(builder.Services, builder.HostEnvironment, builder.Configuration);

            await builder.Build().RunAsync();
        }

        private static void ConfigureServices(IServiceCollection services, IWebAssemblyHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(hostEnvironment.BaseAddress) });

            services.AddHttpClient<RestContentService>("contentapi", client =>
            {
                client.BaseAddress = new Uri(configuration["ContentAPI"]);
            });
        }
    }
}

using BytexDigital.Blazor.Components.CookieConsent;
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

            services.AddCookieConsent(o =>
            {
                o.Revision = 1;
                o.PolicyUrl = "/Cookie-Policy";

                // Call optional
                o.UseDefaultConsentPrompt(prompt =>
                {
                    prompt.Position = ConsentModalPosition.BottomRight;
                    prompt.Layout = ConsentModalLayout.Bar;
                    prompt.SecondaryActionOpensSettings = false;
                    prompt.AcceptAllButtonDisplaysFirst = false;
                });

                o.Categories.Add(new CookieCategory
                {
                    TitleText = new()
                    {
                        ["en"] = "Google Services"
                    },
                    DescriptionText = new()
                    {
                        ["en"] = "Allows the integration and usage of Google services."
                    },
                    Identifier = "google",
                    IsPreselected = true,

                    Services = new()
                    {
                        new CookieCategoryService
                        {
                            Identifier = "google-analytics",
                            PolicyUrl = "https://policies.google.com/privacy",
                            TitleText = new()
                            {
                                ["en"] = "Google Analytics"
                            },
                            ShowPolicyText = new()
                            {
                                ["en"] = "Display policies"
                            }
                        }
                    }
                });
            });
        }
    }
}

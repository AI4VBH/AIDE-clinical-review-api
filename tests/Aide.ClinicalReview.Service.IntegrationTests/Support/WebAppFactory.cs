using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Aide.ClinicalReview.Service.IntegrationTests.Support
{
    public static class WebAppFactory
    {
        public static WebApplicationFactory<Program> GetWebApplicationFactory() =>
            new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureAppConfiguration((context, config) =>
                    {
                        config.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Test.json"));
                    });
                });
    }
}

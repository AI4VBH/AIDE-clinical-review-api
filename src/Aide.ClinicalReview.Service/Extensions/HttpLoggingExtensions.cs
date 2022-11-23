using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aide.ClinicalReview.Service.Extensions
{
    public static class HttpLoggingExtensions
    {
        public static IServiceCollection AddHttpLoggingForClinicalReview(this IServiceCollection services, IConfiguration configuration)
        {
            Guard.Against.Null(configuration, nameof(configuration));

            services.AddHttpLogging(options =>
            {
                options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestPropertiesAndHeaders |
                                        Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
                if (configuration.GetValue<bool>("Kestrel:LogHttpRequestBody", false))
                {
                    options.LoggingFields |= Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestBody;
                }
                if (configuration.GetValue<bool>("Kestrel:LogHttpResponseBody", false))
                {
                    options.LoggingFields |= Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponseBody;
                }
                if (configuration.GetValue<bool>("Kestrel:LogHttpRequestQuery", false))
                {
                    options.LoggingFields |= Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestQuery;
                }
            });

            return services;
        }
    }
}

using Microsoft.Extensions.Configuration;

namespace Aide.ClinicalReview.Configuration
{
    public sealed class EndpointSettings
    {
        [ConfigurationKeyName("defaultPageSize")]
        public int DefaultPageSize { get; set; } = 10;

        [ConfigurationKeyName("maxPageSize")]
        public int MaxPageSize { get; set; } = 10;
    }
}

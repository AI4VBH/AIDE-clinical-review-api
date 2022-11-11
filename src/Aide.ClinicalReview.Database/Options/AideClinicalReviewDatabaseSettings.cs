using Microsoft.Extensions.Configuration;

namespace Aide.ClinicalReview.Database.Options
{
    public sealed class AideClinicalReviewDatabaseSettings
    {
        [ConfigurationKeyName("ConnectionString")]
        public string ConnectionString { get; set; } = null!;

        [ConfigurationKeyName("DatabaseName")]
        public string DatabaseName { get; set; } = null!;

        [ConfigurationKeyName("AideClinicalReviewService")]
        public string AideClinicalReviewService { get; set; } = null!;

    }
}

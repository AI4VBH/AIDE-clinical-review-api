using Microsoft.Extensions.Configuration;

namespace Aide.ClinicalReview.Database.Configuration
{
    public sealed class AideClinicalReviewDatabaseSettings
    {
        [ConfigurationKeyName("ConnectionString")]
        public string ConnectionString { get; set; } = null!;

        [ConfigurationKeyName("DatabaseName")]
        public string DatabaseName { get; set; } = null!;

        [ConfigurationKeyName("AideClinicalReviewRecord")]
        public string AideClinicalReviewRecord { get; set; } = null!;

        [ConfigurationKeyName("AideClinicalReviewStudy")]
        public string AideClinicalReviewStudy { get; set; } = null!;

    }
}

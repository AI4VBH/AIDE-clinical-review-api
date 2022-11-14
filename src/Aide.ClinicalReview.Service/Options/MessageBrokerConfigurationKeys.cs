using Microsoft.Extensions.Configuration;

namespace Aide.ClinicalReview.Service.Options
{
    public sealed class MessageBrokerConfigurationKeys
    {
        /// <summary>
        /// Gets or sets the topic for publishing workflow requests.
        /// Defaults to `md.workflow.request`.
        /// </summary>
        [ConfigurationKeyName("aideClinicalReviewRequest")]
        public string AideClinicalReviewRequest { get; set; } = "aide.clinical_review.request";
    }
}
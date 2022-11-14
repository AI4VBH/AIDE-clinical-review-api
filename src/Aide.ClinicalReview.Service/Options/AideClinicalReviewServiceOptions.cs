using Microsoft.Extensions.Configuration;

namespace Aide.ClinicalReview.Service.Options
{
    public sealed class AideClinicalReviewServiceOptions
    {
        /// <summary>
        /// Represents the <c>messaging</c> section of the configuration file.
        /// </summary>
        [ConfigurationKeyName("messaging")]
        public MessageBrokerConfiguration Messaging { get; set; }

        public AideClinicalReviewServiceOptions()
        {
            Messaging = new MessageBrokerConfiguration();
        }
    }
}
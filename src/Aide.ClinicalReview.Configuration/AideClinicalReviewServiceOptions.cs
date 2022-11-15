using Microsoft.Extensions.Configuration;

namespace Aide.ClinicalReview.Configuration
{
    public sealed class AideClinicalReviewServiceOptions
    {
        /// <summary>
        /// Represents the <c>messaging</c> section of the configuration file.
        /// </summary>
        [ConfigurationKeyName("messaging")]
        public MessageBrokerConfiguration Messaging { get; set; }

        // <summary>
        /// Represents the <c>endpointSettings</c> section of the configuration file.
        /// </summary>
        [ConfigurationKeyName("endpointSettings")]
        public EndpointSettings EndpointSettings { get; set; }

        public AideClinicalReviewServiceOptions()
        {
            Messaging = new MessageBrokerConfiguration();
            EndpointSettings = new EndpointSettings();
        }
    }
}
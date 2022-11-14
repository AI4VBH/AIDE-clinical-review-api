using Microsoft.Extensions.Configuration;
using Monai.Deploy.Messaging.Configuration;
using System.Runtime.InteropServices;

namespace Aide.ClinicalReview.Service.Options
{
    public partial class MessageBrokerConfiguration : MessageBrokerServiceConfiguration
    {
        public static readonly string AideApplicationId = "8B629219-181E-497D-BFE2-1EE96553B8FB";

        /// <summary>
        /// Gets or sets retry options relate to the message broker services.
        /// </summary>
        [ConfigurationKeyName("retries")]
        public RetryConfiguration Retries { get; set; } = new RetryConfiguration();

        /// <summary>
        /// Gets or sets the topics for events published/subscribed by Informatics Gateway
        /// </summary>
        [ConfigurationKeyName("topics")]
        public MessageBrokerConfigurationKeys Topics { get; set; } = new MessageBrokerConfigurationKeys();
    }
}
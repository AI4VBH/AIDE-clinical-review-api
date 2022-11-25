// 
// Copyright 2022 Crown Copyright
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Microsoft.Extensions.Configuration;
using Monai.Deploy.Messaging.Configuration;
using System.Runtime.InteropServices;

namespace Aide.ClinicalReview.Configuration
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
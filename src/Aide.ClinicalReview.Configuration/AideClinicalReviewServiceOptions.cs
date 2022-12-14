// 
// Copyright 2022 Guy’s and St Thomas’ NHS Foundation Trust
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
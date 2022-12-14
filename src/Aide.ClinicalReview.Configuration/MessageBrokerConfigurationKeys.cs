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
    public sealed class MessageBrokerConfigurationKeys
    {
        /// <summary>
        /// Gets or sets the topic for publishing workflow requests.
        /// Defaults to `md.workflow.request`.
        /// </summary>
        [ConfigurationKeyName("aideClinicalReviewRequest")]
        public string AideClinicalReviewRequest { get; set; } = "aide.clinical_review.request";

        /// <summary>
        /// Gets or sets the topic for publishing task callback events.
        /// Defaults to `md.tasks.callback`.
        /// </summary>
        [ConfigurationKeyName("taskCallback")]
        public string TaskCallbackRequest { get; set; } = "md.tasks.callback";
    }
}
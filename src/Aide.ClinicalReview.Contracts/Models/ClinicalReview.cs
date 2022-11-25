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

using Aide.ClinicalReview.Contracts.Messages;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Aide.ClinicalReview.Contracts.Models
{
    public sealed class ClinicalReviewRecord
    {
        [BsonId]
        [JsonProperty("execution_id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("clinical_review_message")]
        public AideClinicalReviewRequestMessage? ClinicalReviewMessage { get; set; }

        [JsonProperty("ready")] public string Ready { get; set; } = string.Empty;

        [JsonProperty("reviewed")] public string Reviewed { get; set; } = string.Empty;

        [JsonProperty("received")] public DateTime Received { get; set; }
    }
}
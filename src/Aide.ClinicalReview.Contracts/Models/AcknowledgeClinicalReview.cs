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

using Newtonsoft.Json;
namespace Aide.ClinicalReview.Contracts.Models
{
    public class AcknowledgeClinicalReview
    {
        [JsonProperty("acceptance")]
        public bool Acceptance { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; } = string.Empty;

        [JsonProperty("message")]
        public string? Message { get; set; }

        [JsonProperty("roles")]
        public string[] Roles { get; set; } = Array.Empty<string>();

        [JsonProperty("user_id")]
        public string UserId { get; set; } = string.Empty;
    }
}

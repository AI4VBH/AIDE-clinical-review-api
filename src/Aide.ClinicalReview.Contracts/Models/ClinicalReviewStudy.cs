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

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Aide.ClinicalReview.Contracts.Models
{
    public class ClinicalReviewStudy
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("executionId")] public string ExecutionId { get; set; } = string.Empty;

        [JsonProperty("roles")] public List<string> Roles { get; set; } = new List<string>();

        [JsonProperty("study_uid")] public string StudyUid { get; set; } = string.Empty;

        [JsonProperty("study_date")]
        public string StudyDate { get; set; } = null;

        [JsonProperty("study_description")] public string StudyDescription { get; set; } = string.Empty;

        [JsonProperty("study")] public List<Series> Study { get; set; } = new List<Series>();
    }

    public class Series
    {
        [JsonProperty("series_uid")] public string SeriesUid { get; set; } = string.Empty;

        [JsonProperty("modality")] public string Modality { get; set; } = string.Empty;

        [JsonProperty("files")] public List<string> Files { get; set; } = new List<string>();
    }
}
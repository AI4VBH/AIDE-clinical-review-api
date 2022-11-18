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

        [JsonProperty("executionId")]
        public string ExecutionId { get; set; } = string.Empty;

        [JsonProperty("roles")]
        public List<string> Roles { get; set; } = new List<string>();

        [JsonProperty("study_uid")]
        public string StudyUid { get; set; } = string.Empty;

        [JsonProperty("study_date")]
        public DateTime? StudyDate { get; set; } = null;

        [JsonProperty("study_description")]
        public string StudyDescription { get; set; } = string.Empty;

        [JsonProperty("study")]
        public List<Series> Study { get; set; } = new List<Series>();
    }

    public class Series
    {
        [JsonProperty("series_uid")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("modality")]
        public string Modality { get; set; } = string.Empty;

        [JsonProperty("files")]
        public List<string> Files { get; set; } = new List<string>();
    }
}

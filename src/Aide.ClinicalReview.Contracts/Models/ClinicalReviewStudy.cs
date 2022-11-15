using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Aide.ClinicalReview.Contracts.Models
{
    public class ClinicalReviewStudy
    {
        [BsonId]
        [JsonPropertyName("execution_id")]
        public string ExecutionId { get; set; } = string.Empty;

        [JsonPropertyName("roles")]
        public List<string> Roles { get; set; } = new List<string>();

        [JsonPropertyName("study")]
        public List<Series> Study { get; set; } = new List<Series>();
    }

    public class Series
    {
        [JsonPropertyName("series_id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("modality")]
        public string Modality { get; set; } = string.Empty;

        [JsonPropertyName("files")]
        public List<string> Files { get; set; } = new List<string>();
    }
}

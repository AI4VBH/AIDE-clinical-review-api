using Aide.ClinicalReview.Contracts.Messages;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Aide.ClinicalReview.Contracts.Models
{
    public sealed class ClinicalReviewRecord
    {
        [BsonId]
        [JsonPropertyName("execution_id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("clinical_review_message")]
        public AideClinicalReviewRequestMessage? ClinicalReviewMessage { get; set; }

        [JsonPropertyName("ready")]
        public string Ready { get; set; } = string.Empty;

        [JsonPropertyName("reviewed")]
        public string Reviewed { get; set; } = string.Empty;
    }
}
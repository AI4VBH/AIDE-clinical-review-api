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

        [JsonProperty("ready")]
        public string Ready { get; set; } = string.Empty;

        [JsonProperty("reviewed")]
        public string Reviewed { get; set; } = string.Empty;

        [JsonProperty("received")]
        public DateTime Received { get; set; }
    }
}
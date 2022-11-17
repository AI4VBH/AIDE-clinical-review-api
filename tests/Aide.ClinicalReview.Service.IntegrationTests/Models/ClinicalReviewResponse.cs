using Aide.ClinicalReview.Contracts.Messages;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Aide.ClinicalReview.Service.IntegrationTests.Models
{
    public class ClinicalReviewResponse
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

        [JsonPropertyName("received")]
        public DateTime Received { get; set; }
    }
}

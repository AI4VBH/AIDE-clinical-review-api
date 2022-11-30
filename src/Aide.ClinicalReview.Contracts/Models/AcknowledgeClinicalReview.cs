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
        public string Message { get; set; } = string.Empty;

        [JsonProperty("roles")]
        public string[] Roles { get; set; } = Array.Empty<string>();

        [JsonProperty("user_id")]
        public string userId { get; set; } = string.Empty;
    }
}

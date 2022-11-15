using Aide.ClinicalReview.Contracts.Extensions;
using Monai.Deploy.Messaging.Events;
using System.Text.Json.Serialization;

namespace Aide.ClinicalReview.Contracts.Messages
{
    public sealed class AideClinicalReviewRequestMessage : EventBase
    {
        [JsonPropertyName("task_id")]
        public string TaskId { get; set; } = string.Empty;

        [JsonPropertyName("reviewed_task_id")]
        public string ReviewedTaskId { get; set; } = string.Empty;

        [JsonPropertyName("execution_id")]
        public string ExecutionId { get; set; } = string.Empty;

        [JsonPropertyName("reviewed_execution_id")]
        public string ReviewedExecutionId { get; set; } = string.Empty;

        [JsonPropertyName("correlation_id")]
        public string CorrelationId { get; set; } = string.Empty;

        [JsonPropertyName("workflow_name")]
        public string WorkflowName { get; set; } = string.Empty;

        [JsonPropertyName("reviewer_roles")]
        public string[] ReviewerRoles { get; set; } = Array.Empty<string>();

        [JsonPropertyName("patient_metadata")]
        public PatientMetadata? PatientMetadata { get; set; } = null;

        [JsonPropertyName("files")]
        public List<File> Files { get; set; } = List.Empty<File>();

        [JsonPropertyName("application_metadata")]
        public Dictionary<string,string> ApplicationMetadata { get; set; } = new Dictionary<string, string>();
    }

    public sealed class Credentials
    {
        [JsonPropertyName("access_key")]
        public string AccessKey { get; set; } = string.Empty;

        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("session_token")]
        public string SessionToken { get; set; } = string.Empty;
    }

    public sealed class File
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("endpoint")]
        public string Endpoint { get; set; } = string.Empty;

        [JsonPropertyName("bucket")]
        public string Bucket { get; set; } = string.Empty;

        [JsonPropertyName("relative_root_path")]
        public string RelativeRootPath { get; set; } = string.Empty;

        [JsonPropertyName("credentials")]
        public Credentials? Credentials { get; set; } = null;
    }

    public sealed class PatientMetadata
    {
        [JsonPropertyName("patient_name")]
        public string PatientName { get; set; } = string.Empty;

        [JsonPropertyName("patient_id")]
        public string PatientId { get; set; } = string.Empty;

        [JsonPropertyName("patient_dob")]
        public string PatientDob { get; set; } = string.Empty;

        [JsonPropertyName("patient_gender")]
        public string PatientGender { get; set; } = string.Empty;
    }
}
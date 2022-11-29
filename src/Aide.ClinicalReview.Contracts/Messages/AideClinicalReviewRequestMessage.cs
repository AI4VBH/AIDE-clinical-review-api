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

using Aide.ClinicalReview.Contracts.Extensions;
using Monai.Deploy.Messaging.Events;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Aide.ClinicalReview.Contracts.Messages
{
    public sealed class AideClinicalReviewRequestMessage : EventBase
    {
        [JsonProperty("task_id")] public string TaskId { get; set; } = string.Empty;

        [JsonProperty("reviewed_task_id")] public string ReviewedTaskId { get; set; } = string.Empty;

        [JsonProperty("execution_id")] public string ExecutionId { get; set; } = string.Empty;

        [JsonProperty("workflow_instance_id")]
        public string WorkflowInstanceId { get; set; } = string.Empty;

        [JsonProperty("reviewed_execution_id")]
        public string ReviewedExecutionId { get; set; } = string.Empty;

        [JsonProperty("correlation_id")] public string CorrelationId { get; set; } = string.Empty;

        [JsonProperty("workflow_name")] public string WorkflowName { get; set; } = string.Empty;

        [JsonProperty("reviewer_roles")] public string[] ReviewerRoles { get; set; } = Array.Empty<string>();

        [JsonProperty("patient_metadata")] public PatientMetadata? PatientMetadata { get; set; } = null;

        [JsonProperty("files")] public List<File> Files { get; set; } = List.Empty<File>();

        [JsonProperty("application_metadata")] public Dictionary<string, string> ApplicationMetadata { get; set; } = new Dictionary<string, string>();
    }

    public sealed class Credentials
    {
        [JsonProperty("access_key")] public string AccessKey { get; set; } = string.Empty;

        [JsonProperty("access_token")] public string AccessToken { get; set; } = string.Empty;

        [JsonProperty("session_token")] public string SessionToken { get; set; } = string.Empty;
    }

    public sealed class File
    {
        [JsonProperty("name")] public string Name { get; set; } = string.Empty;

        [JsonProperty("endpoint")] public string Endpoint { get; set; } = string.Empty;

        [JsonProperty("bucket")] public string Bucket { get; set; } = string.Empty;

        [JsonProperty("relative_root_path")] public string RelativeRootPath { get; set; } = string.Empty;

        [JsonProperty("credentials")] public Credentials? Credentials { get; set; } = null;
    }

    public sealed class PatientMetadata
    {
        [JsonProperty("patient_name")] public string PatientName { get; set; } = string.Empty;

        [JsonProperty("patient_id")] public string PatientId { get; set; } = string.Empty;

        [JsonProperty("patient_dob")] public string PatientDob { get; set; } = string.Empty;

        [JsonProperty("patient_gender")] public string PatientGender { get; set; } = string.Empty;
    }
}
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

using Aide.ClinicalReview.Contracts.Models;
using Ardalis.GuardClauses;
using Monai.Deploy.Messaging.Events;
using Monai.Deploy.Messaging.Messages;

namespace Aide.ClinicalReview.Common.Mappers
{
    public static class EventMapper
    {
        public static JsonMessage<T> ToJsonMessage<T>(T message, string applicationId, string correlationId) where T : EventBase
        {
            return new JsonMessage<T>(message, applicationId, correlationId);
        }

        public static TaskCallbackEvent ToTaskCallbackEvent(ClinicalReviewRecord clinicalReview,
            AcknowledgeClinicalReview acknowledge)
        {
            Guard.Against.Null(clinicalReview);
            Guard.Against.Null(acknowledge);

            var message = clinicalReview.ClinicalReviewMessage;

            var metadata = new Dictionary<string, object>
            {
                { "acceptance", acknowledge.Acceptance },
                { "user_id", acknowledge.UserId },
                { "roles", acknowledge.Roles }
            };

            if (string.IsNullOrWhiteSpace(acknowledge.Reason) is false)
            {
                metadata.Add("reason", acknowledge.Reason);
            }

            if (string.IsNullOrWhiteSpace(acknowledge.Message) is false)
            {
                metadata.Add("message", acknowledge.Message);
            }

            return new TaskCallbackEvent
            {
                WorkflowInstanceId = message.WorkflowInstanceId,
                TaskId = message.TaskId,
                ExecutionId = message.ExecutionId,
                CorrelationId = message.CorrelationId,
                Identity = message.ExecutionId,
                Metadata = metadata
            };
        }

        public static ExportRequestEvent ToExportRequestEvent(IList<string> dicomImages, string[] exportDestinations, string taskId, string workflowInstanceId, string correlationId)
        {
            Guard.Against.NullOrWhiteSpace(taskId, nameof(taskId));
            Guard.Against.NullOrWhiteSpace(workflowInstanceId, nameof(workflowInstanceId));
            Guard.Against.NullOrWhiteSpace(correlationId, nameof(correlationId));
            Guard.Against.NullOrEmpty(dicomImages, nameof(dicomImages));
            Guard.Against.NullOrEmpty(exportDestinations, nameof(exportDestinations));

            return new ExportRequestEvent
            {
                WorkflowInstanceId = workflowInstanceId,
                ExportTaskId = taskId,
                CorrelationId = correlationId,
                Files = dicomImages,
                Destinations = exportDestinations
            };
        }
    }
}

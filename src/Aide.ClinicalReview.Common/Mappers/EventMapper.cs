using Aide.ClinicalReview.Contracts.Models;
using Ardalis.GuardClauses;
using Monai.Deploy.Messaging.Events;
using Monai.Deploy.Messaging.Messages;
using Monai.Deploy.Storage.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Guard.Against.Null(clinicalReview, nameof(clinicalReview));
            Guard.Against.Null(acknowledge, nameof(acknowledge));

            var message = clinicalReview.ClinicalReviewMessage;

            var metadata = new Dictionary<string, object>
            {
                { "acceptance", acknowledge.Acceptance },
                { "user_id", acknowledge.userId },
                { "roles", acknowledge.Roles }
            };

            if (acknowledge.Reason is not null)
            {
                metadata.Add("reason", acknowledge.Reason);
            }

            if (acknowledge.Message is not null)
            {
                metadata.Add("message", acknowledge.Message);
            }

            return new TaskCallbackEvent
            {
                WorkflowInstanceId = message.WorkflowInstanceId,
                TaskId = message.TaskId,
                ExecutionId = message.ExecutionId,
                CorrelationId = message.CorrelationId,
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

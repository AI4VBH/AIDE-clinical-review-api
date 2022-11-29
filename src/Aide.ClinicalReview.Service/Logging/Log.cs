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

using Microsoft.Extensions.Logging;

namespace Aide.ClinicalReview.Service.Logging
{
    public static partial class Log
    {
        [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "{ServiceName} started.")]
        public static partial void ServiceStarted(this ILogger logger, string serviceName);

        [LoggerMessage(EventId = 2, Level = LogLevel.Information, Message = "{ServiceName} is stopping.")]
        public static partial void ServiceStopping(this ILogger logger, string serviceName);


        [LoggerMessage(EventId = 4, Level = LogLevel.Warning, Message = "{ServiceName} canceled.")]
        public static partial void ServiceCancelledWithException(this ILogger logger, string serviceName, Exception ex);

        [LoggerMessage(EventId = 100, Level = LogLevel.Error, Message = "Error processing message, message ID={messageId}, correlation ID={correlationId}.")]
        public static partial void ErrorProcessingMessage(this ILogger logger, string? messageId, string? correlationId, Exception ex);

        [LoggerMessage(EventId = 102, Level = LogLevel.Warning, Message = "Invalid message received, message ID={messageId}, correlation ID={correlationId}.")]
        public static partial void InvalidMessageReceived(this ILogger logger, string? messageId, string? correlationId, Exception ex);

        [LoggerMessage(EventId = 104, Level = LogLevel.Debug, Message = "Sending Nack message for {eventType} without re-queueing.")]
        public static partial void SendingRejectMessageNoRequeue(this ILogger logger, string? eventType);

        [LoggerMessage(EventId = 105, Level = LogLevel.Information, Message = "Nack message sent for {eventType} without re-queueing.")]
        public static partial void RejectMessageNoRequeueSent(this ILogger logger, string eventType);

        [LoggerMessage(EventId = 108, Level = LogLevel.Error, Message = "Error sending message {eventType}.")]
        public static partial void ErrorSendingMessage(this ILogger logger, string eventType, Exception ex);

        [LoggerMessage(EventId = 119, Level = LogLevel.Error, Message = "Recovering connection to storage service: {reason}.")]
        public static partial void MessagingServiceErrorRecover(this ILogger logger, string reason);

        [LoggerMessage(EventId = 120, Level = LogLevel.Error, Message = "Dicom Exception {reason}")]
        public static partial void DicomException(this ILogger logger, string reason, Exception ex);

        [LoggerMessage(EventId = 121, Level = LogLevel.Error, Message = "Unexpected error occurred in GET /clinical-review API.")]
        public static partial void ClinicalReviewGetAllAsyncError(this ILogger logger, Exception ex);

        [LoggerMessage(EventId = 121, Level = LogLevel.Error, Message = "Unexpected error occured in GET /clinical-review/{executionId} API.")]
        public static partial void GetTaskDetailsAsyncError(this ILogger logger, Guid executionId);
    }
}
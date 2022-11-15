using Microsoft.Extensions.Logging;

namespace Aide.ClinicalReview.Service.Logging
{
#pragma warning disable S125
    // Commented out code to match eventId's with
    // Monai Services when implemented in future

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

        [LoggerMessage(EventId = 119, Level = LogLevel.Error, Message = "Recovering connection to storage service:  {reason}.")]
        public static partial void MessagingServiceErrorRecover(this ILogger logger, string reason);

        [LoggerMessage(EventId = 120, Level = LogLevel.Error, Message = "Unexpected error occurred in GET /clinical-review API.")]
        public static partial void ClinicalReviewGetAllAsyncError(this ILogger logger, Exception ex);
    }
#pragma warning restore S125
}
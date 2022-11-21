using Aide.ClinicalReview.Configuration;
using Aide.ClinicalReview.Contracts.Messages;
using Aide.ClinicalReview.Contracts.Exceptions;
using Aide.ClinicalReview.Service.Handler;
using Aide.ClinicalReview.Service.Logging;
using Aide.ClinicalReview.Service.Models;
using Ardalis.GuardClauses;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Monai.Deploy.Messaging.API;
using Monai.Deploy.Messaging.Common;
using Monai.Deploy.Messaging.Events;
using Monai.Deploy.Messaging.Messages;

namespace Aide.ClinicalReview.Service
{
    public sealed class ClincalReviewService : IHostedService, IAideService
    {        
        private readonly ILogger<ClincalReviewService> _logger;
        private readonly IOptions<AideClinicalReviewServiceOptions> _options;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IServiceScope _scope;
        private readonly CancellationTokenSource _cancellationTokenSource;

        private CancellationToken _cancellationToken;
        private IMessageBrokerPublisherService? _messageBrokerPublisherService;
        private IMessageBrokerSubscriberService? _messageBrokerSubscriberService;

        public ServiceStatus Status { get; set; } = ServiceStatus.Unknown;

        public static string ServiceName => "AIDE Clinical Review Service";

        public ClincalReviewService(
            ILogger<ClincalReviewService> logger,
            IOptions<AideClinicalReviewServiceOptions> options,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options ?? throw new ArgumentNullException(nameof(options));

            _serviceScopeFactory = serviceScopeFactory 
                ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
            _scope = _serviceScopeFactory.CreateScope();

            _cancellationTokenSource = new CancellationTokenSource();

            _messageBrokerPublisherService = null;
            _messageBrokerSubscriberService = null;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;

            _messageBrokerPublisherService = _scope.ServiceProvider
                .GetRequiredService<IMessageBrokerPublisherService>() 
                ?? throw new MongoNotFoundException(nameof(IMessageBrokerPublisherService));

            _messageBrokerSubscriberService = _scope.ServiceProvider
                .GetRequiredService<IMessageBrokerSubscriberService>() 
                ?? throw new MongoNotFoundException(nameof(IMessageBrokerSubscriberService));

            _messageBrokerSubscriberService.OnConnectionError += (sender, args) =>
            {
                _logger.MessagingServiceErrorRecover(args.ErrorMessage);
                SubscribeToEvents();
            };

            SubscribeToEvents();

            Status = ServiceStatus.Running;
            _logger.ServiceStarted(ServiceName);
            return Task.CompletedTask;
        }

        private void SubscribeToEvents()
        {
            Guard.Against.Null(_messageBrokerSubscriberService);

            string topic = _options.Value.Messaging.Topics.AideClinicalReviewRequest;
            _messageBrokerSubscriberService.SubscribeAsync(
                topic,
                topic,
                AideClinicalReviewRequestCallBack);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.ServiceStopping(ServiceName);

            _messageBrokerSubscriberService?.Dispose();
            _messageBrokerPublisherService?.Dispose();

            _cancellationTokenSource.Cancel();
            Status = ServiceStatus.Stopped;

            return Task.CompletedTask;
        }

        /// <summary>
        /// Aide Clinical Review Request Call Back.
        /// </summary>
        /// <param name="args">Received message.</param>
        /// <returns></returns>
        public async Task AideClinicalReviewRequestCallBack(MessageReceivedEventArgs args)
        {
            Guard.Against.Null(args);

            using var loggingScope = _logger.BeginScope(new Dictionary<string, object>
            {
                ["correlationId"] = args.Message.CorrelationId,
                ["messageId"] = args.Message.MessageId,
                ["messageType"] = args.Message.MessageDescription
            });

            try
            {
                var message = args.Message.ConvertToJsonMessage<AideClinicalReviewRequestMessage>();

                var handler = _scope.ServiceProvider.GetService<ICallBackHandler<AideClinicalReviewRequestMessage>>()
                    ?? throw new MongoNotFoundException(nameof(ReviewRequestCallBackHandler));

                // Run action on the message
                await handler.HandleMessage(message).ConfigureAwait(false);
            }
            catch (OperationCanceledException ex)
            {
                _logger.ServiceCancelledWithException(ServiceName, ex);
                await HandleMessageException<AideClinicalReviewRequestMessage>(args.Message, true).ConfigureAwait(false);
            }
            catch (MessageValidationException ex)
            {
                _logger.InvalidMessageReceived(args.Message.MessageId, args.Message.CorrelationId, ex);
                await HandleMessageException<AideClinicalReviewRequestMessage>(args.Message, false).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.ErrorProcessingMessage(args.Message.MessageId, args.Message.CorrelationId, ex);
                await HandleMessageException<AideClinicalReviewRequestMessage>(args.Message, true).ConfigureAwait(false);
            }
        }

        public async Task HandleMessageException<T>(Message message, bool requeue) where T : EventBase
        {
            if (message is null)
            {
                return;
            }

            var taskId = "";
            var executionId = "";
            try
            {
                var jsonMessage = message.ConvertToJsonMessage<T>();
                if (jsonMessage.Body is AideClinicalReviewRequestMessage tmessage)
                {
                    taskId = tmessage.TaskId;
                    executionId = tmessage.ExecutionId;
                }
            }
            catch (Exception ex)
            {
                _logger.ErrorProcessingMessage(message.MessageId, message.CorrelationId, ex);
                return;
            }

            using var loggingScope = _logger.BeginScope(new Dictionary<string, object>
            {
                ["correlationId"] = message.CorrelationId,
                ["messageId"] = message.MessageId,
                ["taskId"] = taskId,
                ["executionId"] = executionId
            });

            try
            {
                _logger.SendingRejectMessageNoRequeue(message.MessageDescription);

                if (requeue)
                {
                    await _messageBrokerSubscriberService!.RequeueWithDelay(message);
                }
                else
                {
                    _messageBrokerSubscriberService!.Reject(message, false);
                }

                _logger.RejectMessageNoRequeueSent(message.MessageDescription);
            }
            catch (Exception ex)
            {
                _logger.ErrorSendingMessage(message.MessageDescription, ex);
            }
        }
    }
}
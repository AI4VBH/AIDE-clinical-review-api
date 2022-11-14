using Aide.ClinicalReview.Contracts.Messages;
using Aide.ClinicalReview.Contracts.Models;
using Aide.ClinicalReview.Database.Interfaces;
using Ardalis.GuardClauses;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Monai.Deploy.Messaging.Messages;

namespace Aide.ClinicalReview.Service.Handler
{
    public sealed class ReviewRequestCallBackHandler : ICallBackHandler<AideClinicalReviewRequestMessage>
    {
        private readonly IServiceScope _scope;
        private readonly ILogger<ReviewRequestCallBackHandler> _logger;
        private readonly IClinicalReviewRepository _clinicalReviewRepository;

        public ReviewRequestCallBackHandler(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<ReviewRequestCallBackHandler> logger,
            IClinicalReviewRepository clinicalReviewRepository)
        {
            Guard.Against.Null(serviceScopeFactory);
            Guard.Against.Null(logger);
            Guard.Against.Null(clinicalReviewRepository);

            _scope = serviceScopeFactory.CreateScope();
            _logger = logger;
            _clinicalReviewRepository = clinicalReviewRepository;
        }

        public async Task HandleMessage(JsonMessage<AideClinicalReviewRequestMessage> message)
        {
            Guard.Against.Null(message);

            message.Body.Validate();

            var readyState = "false";
            var reviewed = "false";

            var clinicalReviewRecord = new ClinicalReviewRecord()
            {
                ClinicalReviewMessage = message.Body, 
                Ready = readyState, 
                Reviewed = reviewed
            };

            await _clinicalReviewRepository.CreateAsync(clinicalReviewRecord);

        }
    }
}
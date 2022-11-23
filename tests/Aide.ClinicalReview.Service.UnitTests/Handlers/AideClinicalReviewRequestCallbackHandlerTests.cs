using Aide.ClinicalReview.Database.Interfaces;
using Aide.ClinicalReview.Service.Handler;
using Aide.ClinicalReview.Service.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Aide.ClinicalReview.Service.UnitTests.Handlers
{
    public class AideClinicalReviewRequestCallbackHandlerTests
    {
        private ReviewRequestCallBackHandler AideClinicalReviewCallbackHandler { get; set; }

        private readonly Mock<IServiceScopeFactory> _serviceScopeFactory;
        private readonly Mock<ILogger<ReviewRequestCallBackHandler>> _logger;
        private readonly Mock<IClinicalReviewRepository> _clinicalReviewRepository;
        private readonly Mock<ITaskDetailsRepository> _taskDetailsRepository;
        private readonly Mock<IDicomService> _dicomService;
        
        public AideClinicalReviewRequestCallbackHandlerTests()
        {
            _serviceScopeFactory = new Mock<IServiceScopeFactory>();
            _logger = new Mock<ILogger<ReviewRequestCallBackHandler>>();
            _clinicalReviewRepository = new Mock<IClinicalReviewRepository>();
            _taskDetailsRepository = new Mock<ITaskDetailsRepository>();
            _dicomService = new Mock<IDicomService>();

            AideClinicalReviewCallbackHandler = new ReviewRequestCallBackHandler(
                _serviceScopeFactory.Object,
                _logger.Object,
                _clinicalReviewRepository.Object,
                _taskDetailsRepository.Object,
                _dicomService.Object
            );
        }

        [Fact]
        public async void HandleMessage_NullMessage_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AideClinicalReviewCallbackHandler.HandleMessage(null));
        }
    }
}

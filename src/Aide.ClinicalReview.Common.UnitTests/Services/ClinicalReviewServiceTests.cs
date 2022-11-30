using Aide.ClinicalReview.Common.Interfaces;
using Aide.ClinicalReview.Common.Services;
using Aide.ClinicalReview.Configuration;
using Aide.ClinicalReview.Contracts.Exceptions;
using Aide.ClinicalReview.Contracts.Messages;
using Aide.ClinicalReview.Contracts.Models;
using Aide.ClinicalReview.Database.Interfaces;
using Aide.ClinicalReview.Database.Repository;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Monai.Deploy.Messaging.API;
using Monai.Deploy.Messaging.Messages;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Aide.ClinicalReview.Common.UnitTests.Services
{
    public class ClinicalReviewServiceTests
    {
        private ClinicalReviewService ClinicalReviewService { get; }

        private readonly Mock<IClinicalReviewRepository> _clinicalReviewRepository;

        private readonly Mock<IMessageBrokerPublisherService> _publisherService;

        private IOptions<AideClinicalReviewServiceOptions> _options;

        private Mock<ILogger<ClinicalReviewService>> _logger;

        public ClinicalReviewServiceTests()
        {
            _clinicalReviewRepository = new Mock<IClinicalReviewRepository>();
            _publisherService = new Mock<IMessageBrokerPublisherService>();
            _logger = new Mock<ILogger<ClinicalReviewService>>();
            _options = Options.Create(new AideClinicalReviewServiceOptions());

            ClinicalReviewService = new ClinicalReviewService(_clinicalReviewRepository.Object, _publisherService.Object, _options, _logger.Object);
        }

        [Fact]
        public async Task AcknowledgeClinicalReview_NotFound_ThrowsMongoNotFoundException()
        {
            var executionId = Guid.NewGuid().ToString();

            var acknowledge = new AcknowledgeClinicalReview
            {
                Acceptance = true,
                Message = "message",
                Roles = new string[] { "clinician" },
                userId = "jack"
            };

            await Assert.ThrowsAsync<MongoNotFoundException>(() => ClinicalReviewService.AcknowledgeClinicalReview(executionId, acknowledge));
        }

        [Fact]
        public async Task AcknowledgeClinicalReview_UnauthorisedRole_ThrowsUnauthorisedRoleException()
        {
            var executionId = Guid.NewGuid().ToString();

            var acknowledge = new AcknowledgeClinicalReview
            {
                Acceptance = true,
                Message = "message",
                Roles = new string[] { "clinician" },
                userId = "jack"
            };

            var clinicalReview = new ClinicalReviewRecord
            {
                Id = executionId,
                Ready= true,
                Received = DateTime.UtcNow,
                ClinicalReviewMessage = new AideClinicalReviewRequestMessage
                {
                    ExecutionId = executionId,
                    ReviewerRoles = new string[] {"admin"}
                }
            };

            _clinicalReviewRepository.Setup(x => x.GetByClinicalReviewIdAsync(executionId)).ReturnsAsync(clinicalReview);

            await Assert.ThrowsAsync<UnathorisedRoleException>(() => ClinicalReviewService.AcknowledgeClinicalReview(executionId, acknowledge));
        }

        [Fact]
        public async Task AcknowledgeClinicalReview_PreviouslyReviewed_ThrowsPreviouslyReviewedException()
        {
            var executionId = Guid.NewGuid().ToString();

            var acknowledge = new AcknowledgeClinicalReview
            {
                Acceptance = true,
                Message = "message",
                Roles = new string[] { "clinician" },
                userId = "jack"
            };

            var clinicalReview = new ClinicalReviewRecord
            {
                Id = executionId,
                Ready = true,
                Reviewed= DateTime.UtcNow,
                Received = DateTime.UtcNow,
                ClinicalReviewMessage = new AideClinicalReviewRequestMessage
                {
                    ExecutionId = executionId,
                    ReviewerRoles = new string[] { "clinician" }
                }
            };

            _clinicalReviewRepository.Setup(x => x.GetByClinicalReviewIdAsync(executionId)).ReturnsAsync(clinicalReview);

            await Assert.ThrowsAsync<PreviouslyReviewedException>(() => ClinicalReviewService.AcknowledgeClinicalReview(executionId, acknowledge));
        }

        [Fact]
        public async Task AcknowledgeClinicalReview_ValidClinicalReview_PublishesMessage()
        {
            var executionId = Guid.NewGuid().ToString();

            var acknowledge = new AcknowledgeClinicalReview
            {
                Acceptance = true,
                Message = "message",
                Roles = new string[] { "clinician" },
                userId = "jack"
            };

            var clinicalReview = new ClinicalReviewRecord
            {
                Id = executionId,
                Ready = true,
                Received = DateTime.UtcNow,
                ClinicalReviewMessage = new AideClinicalReviewRequestMessage
                {
                    ExecutionId = executionId,
                    ReviewerRoles = new string[] { "clinician" },
                    CorrelationId = Guid.NewGuid().ToString(),
                    ReviewedExecutionId = Guid.NewGuid().ToString(),
                    ReviewedTaskId= Guid.NewGuid().ToString(),
                    TaskId  = Guid.NewGuid().ToString(),
                    WorkflowInstanceId = Guid.NewGuid().ToString(),
                }
            };

            _clinicalReviewRepository.Setup(x => x.GetByClinicalReviewIdAsync(executionId)).ReturnsAsync(clinicalReview);

            await ClinicalReviewService.AcknowledgeClinicalReview(executionId, acknowledge);

            _publisherService.Verify(w => w.Publish(It.IsAny<string>(), It.IsAny<Message>()), Times.Exactly(1));
        }
    }
}

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

using Aide.ClinicalReview.Common.Interfaces;
using Aide.ClinicalReview.Common.Mappers;
using Aide.ClinicalReview.Configuration;
using Aide.ClinicalReview.Contracts.Exceptions;
using Aide.ClinicalReview.Contracts.Models;
using Aide.ClinicalReview.Database.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Monai.Deploy.Messaging.API;
using Monai.Deploy.Messaging.Configuration;
using Monai.Deploy.Messaging.Events;
using Monai.Deploy.Messaging.Messages;
using System.Data;

namespace Aide.ClinicalReview.Common.Services
{
    public sealed class ClinicalReviewService : IClinicalReviewService
    {
        private readonly IClinicalReviewRepository _clinicalReviewRepository;
        private readonly IMessageBrokerPublisherService _messageBrokerPublisherService;
        private readonly AideClinicalReviewServiceOptions _configuration;
        private readonly ILogger<ClinicalReviewService> _logger;

        private string TaskCallbackRoutingKey { get; }

        public ClinicalReviewService(
            IClinicalReviewRepository clinicalReviewRepository,
            IMessageBrokerPublisherService messageBrokerPublisherService,
            IOptions<AideClinicalReviewServiceOptions> configuration,
            ILogger<ClinicalReviewService> logger)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            _configuration = configuration.Value;
            TaskCallbackRoutingKey = configuration.Value.Messaging.Topics.TaskCallbackRequest;

            _clinicalReviewRepository = clinicalReviewRepository ?? throw new ArgumentNullException(nameof(clinicalReviewRepository));
            _messageBrokerPublisherService = messageBrokerPublisherService ?? throw new ArgumentNullException(nameof(messageBrokerPublisherService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<(IList<ClinicalReviewRecord> ClinicalReviews, long recordCount)> GetClinicalReviewListAsync(string[] roles,
            int? skip = null,
            int? limit = null,
            string? patientId = "",
            string? patientName = "",
            string? applicationName = "")
            => await _clinicalReviewRepository.GetClinicalReviewListAsync(roles, skip, limit, patientId, patientName, applicationName);

        public async Task AcknowledgeClinicalReview(string executionId, AcknowledgeClinicalReview acknowledge) 
        {
            var clinicalReview = await _clinicalReviewRepository.GetByClinicalReviewIdAsync(executionId);

            if (clinicalReview is null || clinicalReview.ClinicalReviewMessage is null)
            {
                throw new MongoNotFoundException($"Clinical review for {executionId} not found.");
            }

            if (clinicalReview.ClinicalReviewMessage.ReviewerRoles.Any(r => acknowledge.Roles.Contains(r, StringComparer.InvariantCultureIgnoreCase)) is false)
            {
                throw new UnathorisedRoleException("Role is unathorised");
            }

            if (clinicalReview.Reviewed is not null)
            {
                throw new PreviouslyReviewedException($"Clinical Review Task Previously Reviewed.");
            }

            var taskCallbackEvent = EventMapper.ToTaskCallbackEvent(clinicalReview, acknowledge);
            var jsonMesssage = new JsonMessage<TaskCallbackEvent>(taskCallbackEvent, MessageBrokerConfiguration.ClinicalReviewServiceApplicationId, taskCallbackEvent.CorrelationId, Guid.NewGuid().ToString());

            await _messageBrokerPublisherService.Publish(TaskCallbackRoutingKey, jsonMesssage.ToMessage());

            await _clinicalReviewRepository.AcknowledgeAsync(executionId, acknowledge);
        }
    }
}
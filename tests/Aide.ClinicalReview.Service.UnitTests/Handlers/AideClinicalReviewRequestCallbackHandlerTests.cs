// 
// Copyright 2022 Crown Copyright
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
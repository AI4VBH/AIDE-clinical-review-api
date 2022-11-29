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

using Aide.ClinicalReview.Configuration;
using Aide.ClinicalReview.Service.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Monai.Deploy.Messaging.API;
using System;
using System.Threading;

namespace Aide.ClinicalReview.Service.UnitTests
{
    public class TestBase
    {
        public static Mock<ILogger<T>> NewMockLogger<T>() => new();

        public static Mock<IServiceProvider> SetupServiceProvider(Mock<IMessageBrokerPublisherService> mockPub, Mock<IMessageBrokerSubscriberService> mockSub)
        {
            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(typeof(IMessageBrokerPublisherService)))
                .Returns(mockPub.Object);
            serviceProvider
                .Setup(x => x.GetService(typeof(IMessageBrokerSubscriberService)))
                .Returns(mockSub.Object);
            return serviceProvider;
        }
    }

    public sealed class ClincalReviewServiceTests : TestBase
    {
        private IOptions<AideClinicalReviewServiceOptions> _options;
        private Mock<ILogger<ClincalReviewService>> _logger;
        private Mock<IServiceScopeFactory> _scopeFactory;
        private Mock<IServiceScope> _serviceScope;
        private Mock<IMessageBrokerPublisherService> _messageBrokerPublisherService;
        private Mock<IMessageBrokerSubscriberService> _messageBrokerSubscriberService;

        public ClincalReviewServiceTests()
        {
            _logger = NewMockLogger<ClincalReviewService>();
            _options = Options.Create(new AideClinicalReviewServiceOptions());

            _scopeFactory = new Mock<IServiceScopeFactory>();
            _serviceScope = new Mock<IServiceScope>();
            _scopeFactory.Setup(p => p.CreateScope()).Returns(_serviceScope.Object);

            _messageBrokerPublisherService = new Mock<IMessageBrokerPublisherService>();
            _messageBrokerSubscriberService = new Mock<IMessageBrokerSubscriberService>();

            var serviceProvider = SetupServiceProvider(_messageBrokerPublisherService, _messageBrokerSubscriberService);
            _serviceScope.Setup(x => x.ServiceProvider).Returns(serviceProvider.Object);
        }

        [Fact]
        public async void ClincalReviewService_Starts()
        {
            var service = new ClincalReviewService(_logger.Object, _options, _scopeFactory.Object);
            CancellationTokenSource cancellationTokenSource = new();

            await service.StartAsync(cancellationTokenSource.Token);

            Assert.Equal(ServiceStatus.Running, service.Status);
        }
    }
}
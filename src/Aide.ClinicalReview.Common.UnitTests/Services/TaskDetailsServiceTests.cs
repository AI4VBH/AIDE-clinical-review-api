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

using Aide.ClinicalReview.Common.Services;
using Aide.ClinicalReview.Contracts.Models;
using Aide.ClinicalReview.Database.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Aide.ClinicalReview.Common.UnitTests.Services
{
    public class TaskDetailsServiceTests
    {
        private TaskDetailsService TaskDetailsService { get; }

        private readonly Mock<ITaskDetailsRepository> _taskDetailsRepository;

        public TaskDetailsServiceTests()
        {
            _taskDetailsRepository = new Mock<ITaskDetailsRepository>();

            TaskDetailsService = new TaskDetailsService(_taskDetailsRepository.Object);
        }

        [Fact]
        public async Task GetTaskDetailsAsync_InvalidGuid_ThrowsException()
        {
            var roles = new string[] { "clinician" };
            await Assert.ThrowsAsync<ArgumentException>(() => TaskDetailsService.GetTaskDetailsAsync(Guid.Empty, roles));
        }

        [Fact]
        public async Task GetTaskDetailsAsync_ValidGuid_ReturnsExpectedResult()
        {
            _taskDetailsRepository.Setup(x => x.GetTaskDetailsAsync(It.IsAny<Guid>())).ReturnsAsync(new ClinicalReviewStudy() { Roles = new List<string> { "clinician" } });

            var roles = new string[] { "clinician" };
            var result = await TaskDetailsService.GetTaskDetailsAsync(Guid.NewGuid(), roles);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task CreateTaskDetailsAsync_NullMessage_ThrowsException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => TaskDetailsService.CreateTaskDetailsAsync(null));
        }

        [Fact]
        public async Task CreateTaskDetailsAsync_DefaultMessage_ReturnsDefaultClinicalReviewStudy()
        {
            _taskDetailsRepository.Setup(x => x.CreateTaskDetailsAsync(It.IsAny<ClinicalReviewStudy>())).ReturnsAsync(Guid.NewGuid().ToString());

            var result = await TaskDetailsService.CreateTaskDetailsAsync(new ClinicalReviewStudy());

            Assert.NotNull(result);
        }
    }
}
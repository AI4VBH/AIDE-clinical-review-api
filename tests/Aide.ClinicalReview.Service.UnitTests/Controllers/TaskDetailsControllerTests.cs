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
using Aide.ClinicalReview.Configuration;
using Aide.ClinicalReview.Contracts.Exceptions;
using Aide.ClinicalReview.Contracts.Models;
using Aide.ClinicalReview.Service.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Aide.ClinicalReview.Service.UnitTests.Controllers
{
    public class TaskDetailsControllerTests
    {
        private TaskDetailsController TaskDetailsController { get; set; }

        private readonly Mock<ITaskDetailsService> _taskDetailsService;
        private readonly Mock<ILogger<TaskDetailsController>> _logger;
        private readonly Mock<IOptions<AideClinicalReviewServiceOptions>> _options;

        public TaskDetailsControllerTests()
        {
            _taskDetailsService = new Mock<ITaskDetailsService>();
            _logger = new Mock<ILogger<TaskDetailsController>>();
            _options = new Mock<IOptions<AideClinicalReviewServiceOptions>>();

            TaskDetailsController = new TaskDetailsController(_taskDetailsService.Object, _options.Object, _logger.Object);
        }

        [Fact]
        public async Task GetTaskDetailsAsync_WrongExecutionId_ReturnsNotFoundExecutionId()
        {
            var roles = new string[] { "clinician" };
            _taskDetailsService.Setup(x => x.GetTaskDetailsAsync(It.IsAny<Guid>(), It.IsAny<string[]>())).ThrowsAsync(new MongoNotFoundException("Not found executionId"));

            var result = await TaskDetailsController.GetTaskDetailsAsync(Guid.NewGuid(), "clincian");

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetTaskDetailsAsync_DefaultExecutionId_ReturnsBadRequest()
        {
            var roles = new string[] { "clinician" };
            _taskDetailsService.Setup(x => x.GetTaskDetailsAsync(Guid.Empty, It.IsAny<string[]>())).ThrowsAsync(new ArgumentException());

            var result = await TaskDetailsController.GetTaskDetailsAsync(Guid.Empty, "clincian");

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

        public async Task GetTaskDetailsAsync_ThrowsException_ReturnsBadRequestRoles()
        {
            var roles = new string[] { "clinician" };
            _taskDetailsService.Setup(x => x.GetTaskDetailsAsync(It.IsAny<Guid>(), It.IsAny<string[]>())).ThrowsAsync(new UnathorisedRoleException("Unauthorised Roles"));

            var result = await TaskDetailsController.GetTaskDetailsAsync(Guid.NewGuid(), "clincian");

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

        // TODO
        // Add test that checks the endpoint is available to users with the clinician role only
        // Requires auth middleware

        [Fact]
        public async Task GetTaskDetailsAsync_ThrowsException_ReturnsInternalServerError()
        {
            var roles = new string[] { "clinician" };
            _taskDetailsService.Setup(x => x.GetTaskDetailsAsync(It.IsAny<Guid>(), It.IsAny<string[]>())).Throws(() => new Exception("unexpected error"));

            var result = await TaskDetailsController.GetTaskDetailsAsync(Guid.NewGuid(), "clincian");

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetTaskDetailsAsync_ValidExecutionId_ReturnsExpectedResult()
        {
            var roles = new string[] { "clinician" };
            _taskDetailsService.Setup(x => x.GetTaskDetailsAsync(It.IsAny<Guid>(), roles)).ReturnsAsync(new ClinicalReviewStudy());

            var result = await TaskDetailsController.GetTaskDetailsAsync(Guid.NewGuid(), "clincian");

            Assert.IsType<OkObjectResult>(result);
        }
    }
}
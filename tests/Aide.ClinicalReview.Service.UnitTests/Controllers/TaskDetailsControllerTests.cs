using Aide.ClinicalReview.Common.Interfaces;
using Aide.ClinicalReview.Configuration;
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
        public async Task GetTaskDetailsAsync_WrongExecutionId_ReturnsBadRequest()
        {
            _taskDetailsService.Setup(x => x.GetTaskDetailsAsync(It.IsAny<Guid>())).ReturnsAsync((ClinicalReviewStudy)null);

            var result = await TaskDetailsController.GetTaskDetailsAsync(Guid.NewGuid());

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        }

        // TODO
        // Add test that checks the endpoint is available to users with the clinician role only
        // Requires auth middleware

        [Fact]
        public async Task GetTaskDetailsAsync_ThrowsException_ReturnsInternalServerError()
        {
            _taskDetailsService.Setup(x => x.GetTaskDetailsAsync(It.IsAny<Guid>())).Throws(() => new Exception("unexpected error"));

            var result = await TaskDetailsController.GetTaskDetailsAsync(Guid.NewGuid());

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetTaskDetailsAsync_ValidExecutionId_ReturnsExpectedResult()
        {
            _taskDetailsService.Setup(x => x.GetTaskDetailsAsync(It.IsAny<Guid>())).ReturnsAsync(new ClinicalReviewStudy());

            var result = await TaskDetailsController.GetTaskDetailsAsync(Guid.NewGuid());

            Assert.IsType<OkObjectResult>(result);
        }
    }
}

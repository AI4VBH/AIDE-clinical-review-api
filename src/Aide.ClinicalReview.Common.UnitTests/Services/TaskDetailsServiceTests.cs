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
            var roles = new string[] { "clinician"};
            await Assert.ThrowsAsync<ArgumentException>(() => TaskDetailsService.GetTaskDetailsAsync(Guid.Empty,roles));
        }

        [Fact]
        public async Task GetTaskDetailsAsync_ValidGuid_ReturnsExpectedResult()
        {
            _taskDetailsRepository.Setup(x => x.GetTaskDetailsAsync(It.IsAny<Guid>())).ReturnsAsync(new ClinicalReviewStudy() { Roles = new List<string> { "clinician" } });

            var roles = new string[] { "clinician" };
            var result = await TaskDetailsService.GetTaskDetailsAsync(Guid.NewGuid(),roles);

            Assert.NotNull(result);
        }
    }
}

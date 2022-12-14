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
using Aide.ClinicalReview.Contracts.Messages;
using Aide.ClinicalReview.Contracts.Models;
using Aide.ClinicalReview.Service.Controllers;
using Aide.ClinicalReview.Service.Services;
using Aide.ClinicalReview.Service.Wrappers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aide.ClinicalReview.Service.UnitTests.Controllers
{
    public sealed class ClinicalReviewControllerTests
    {
        private ClinicalReviewController ClinicalReviewController { get; set; }

        private readonly Mock<IClinicalReviewService> _clinicalReviewService;
        private readonly Mock<ILogger<ClinicalReviewController>> _logger;
        private readonly Mock<IUriService> _uriService;
        private readonly IOptions<AideClinicalReviewServiceOptions> _options;

        public ClinicalReviewControllerTests()
        {
            _options = Options.Create(new AideClinicalReviewServiceOptions() { EndpointSettings = new EndpointSettings { DefaultPageSize = 10, MaxPageSize = 500 } });
            _clinicalReviewService = new Mock<IClinicalReviewService>();
            _logger = new Mock<ILogger<ClinicalReviewController>>();
            _uriService = new Mock<IUriService>();

            ClinicalReviewController = new ClinicalReviewController(_clinicalReviewService.Object, _options, _uriService.Object, _logger.Object);
        }

        #region GetAllAsync

        [Fact]
        public async Task GetListAsync_PayloadsExist_ReturnsList()
        {
            var clinicalReviews = new List<ClinicalReviewRecord>
            {
                new ClinicalReviewRecord
                {
                    Id = Guid.NewGuid().ToString(),
                    ClinicalReviewMessage =
                        new AideClinicalReviewRequestMessage()
                        {
                            CorrelationId = "123",
                            ExecutionId = Guid.NewGuid().ToString(),
                            ReviewedExecutionId = "abc",
                            ReviewedTaskId = "cde",
                            WorkflowName = "bobwf",
                            ApplicationMetadata = new Dictionary<string, string> { { "application_name", "test value" }, { "application_version", "test value" } },
                            Files = new List<File>
                            {
                                new File()
                                {
                                    Bucket = "bucket",
                                    Credentials = new Credentials
                                    {
                                        AccessKey = "accesskey",
                                        AccessToken = "token",
                                        SessionToken = "token"
                                    }
                                }
                            },
                            PatientMetadata = new PatientMetadata()
                            {
                                PatientDob = "this is dob",
                                PatientSex = "F",
                                PatientId = "id",
                                PatientName = "pizza"
                            },
                            TaskId = "taskid",
                            ReviewerRoles = new string[] { "clinician" }
                        }
                }
            };

            _clinicalReviewService.Setup(w => w.GetClinicalReviewListAsync(It.IsAny<string[]>(), It.IsAny<int>(), It.IsAny<int>(), "", "", "")).ReturnsAsync((clinicalReviews, 1));
            _uriService.Setup(s => s.GetPageUriString(It.IsAny<Filter.PaginationFilter>(), It.IsAny<string>())).Returns(() => "unitTest");

            var result = await ClinicalReviewController.GetAllAsync(new Filter.PaginationFilter(), "clinician,admin");

            var objectResult = Assert.IsType<OkObjectResult>(result);

            var responseValue = (PagedResponse<List<ClinicalReviewRecord>>)objectResult.Value;
            responseValue.Data.Should().BeEquivalentTo(clinicalReviews);
            responseValue.FirstPage.Should().Be("unitTest");
            responseValue.LastPage.Should().Be("unitTest");
            responseValue.PageNumber.Should().Be(1);
            responseValue.PageSize.Should().Be(10);
            responseValue.TotalPages.Should().Be(1);
            responseValue.TotalRecords.Should().Be(1);
            responseValue.Succeeded.Should().Be(true);
            responseValue.PreviousPage.Should().Be(null);
            responseValue.NextPage.Should().Be(null);
            responseValue.Errors.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetListAsync_MissingRoles_ReturnsBadRequest()
        {
            var result = await ClinicalReviewController.GetAllAsync(new Filter.PaginationFilter(), "");

            var objectResult = Assert.IsType<ObjectResult>(result);

            objectResult.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task GetListAsync_ThrowsException_ReturnsInternalServerError()
        {
            _clinicalReviewService.Setup(w => w.GetClinicalReviewListAsync(It.IsAny<string[]>(), It.IsAny<int>(), It.IsAny<int>(), "", "", "")).Throws(() => new Exception("unexpected error"));

            var result = await ClinicalReviewController.GetAllAsync(new Filter.PaginationFilter(), "clinician");

            var objectResult = Assert.IsType<ObjectResult>(result);

            objectResult.StatusCode.Should().Be(500);
        }

        #endregion

        #region AcknowledgeClinicalReview

        [Fact]
        public async Task AcknowledgeClinicalReview_ValidAcknowledge_Returns204()
        {
            var executionId = Guid.NewGuid().ToString();

            var acknowledge = new AcknowledgeClinicalReview
            {
                Acceptance = true,
                Message = "message",
                Roles = new string[] { "clinician" },
                userId = "jack"
            };

            var result = await ClinicalReviewController.AcknowledgeClinicalReview(executionId, acknowledge);

            var objectResult = Assert.IsType<StatusCodeResult>(result);

            objectResult.StatusCode.Should().Be(204);
        }

        [Fact]
        public async Task AcknowledgeClinicalReview_InvalidExecutionId_ReturnsBadRequest()
        {
            var executionId = "";

            var acknowledge = new AcknowledgeClinicalReview
            {
                Acceptance = true,
                Message = "message",
                Roles = new string[] { "clinician" },
                userId = "jack"
            };

            var result = await ClinicalReviewController.AcknowledgeClinicalReview(executionId, acknowledge);

            var objectResult = Assert.IsType<ObjectResult>(result);

            objectResult.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task AcknowledgeClinicalReview_InvalidBody_ReturnsBadRequest()
        {
            var executionId = Guid.NewGuid().ToString();

            var acknowledge = new AcknowledgeClinicalReview
            {
                Acceptance = false,
                Message = "message",
                Roles = new string[] { "clinician" },
                userId = "jack"
            };

            var result = await ClinicalReviewController.AcknowledgeClinicalReview(executionId, acknowledge);

            var objectResult = Assert.IsType<ObjectResult>(result);

            objectResult.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task AcknowledgeClinicalReview_PreviouslyReviewed_ReturnsBadRequest()
        {
            var executionId = Guid.NewGuid().ToString();

            var acknowledge = new AcknowledgeClinicalReview
            {
                Acceptance = true,
                Message = "message",
                Roles = new string[] { "clinician" },
                userId = "jack"
            };

            _clinicalReviewService.Setup(w => w.AcknowledgeClinicalReview(executionId, acknowledge)).Throws(() => new PreviouslyReviewedException("unexpected error"));

            var result = await ClinicalReviewController.AcknowledgeClinicalReview(executionId, acknowledge);

            var objectResult = Assert.IsType<ObjectResult>(result);

            objectResult.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task AcknowledgeClinicalReview_UnauthorisedRoles_ReturnsForbidden()
        {
            var executionId = Guid.NewGuid().ToString();

            var acknowledge = new AcknowledgeClinicalReview
            {
                Acceptance = true,
                Message = "message",
                Roles = new string[] { "clinician" },
                userId = "jack"
            };

            _clinicalReviewService.Setup(w => w.AcknowledgeClinicalReview(executionId, acknowledge)).Throws(() => new UnathorisedRoleException("unexpected error"));

            var result = await ClinicalReviewController.AcknowledgeClinicalReview(executionId, acknowledge);

            var objectResult = Assert.IsType<ObjectResult>(result);

            objectResult.StatusCode.Should().Be(403);
        }

        [Fact]
        public async Task AcknowledgeClinicalReview_NotFound_ReturnsNotFound()
        {
            var executionId = Guid.NewGuid().ToString();

            var acknowledge = new AcknowledgeClinicalReview
            {
                Acceptance = true,
                Message = "message",
                Roles = new string[] { "clinician" },
                userId = "jack"
            };

            _clinicalReviewService.Setup(w => w.AcknowledgeClinicalReview(executionId, acknowledge)).Throws(() => new MongoNotFoundException("unexpected error"));

            var result = await ClinicalReviewController.AcknowledgeClinicalReview(executionId, acknowledge);

            var objectResult = Assert.IsType<ObjectResult>(result);

            objectResult.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task AcknowledgeClinicalReview_UnexpectedError_Returns500()
        {
            var executionId = Guid.NewGuid().ToString();

            var acknowledge = new AcknowledgeClinicalReview
            {
                Acceptance = true,
                Message = "message",
                Roles = new string[] { "clinician" },
                userId = "jack"
            };

            _clinicalReviewService.Setup(w => w.AcknowledgeClinicalReview(executionId, acknowledge)).Throws(() => new Exception("unexpected error"));

            var result = await ClinicalReviewController.AcknowledgeClinicalReview(executionId, acknowledge);

            var objectResult = Assert.IsType<ObjectResult>(result);

            objectResult.StatusCode.Should().Be(500);
        }

        #endregion
    }
}
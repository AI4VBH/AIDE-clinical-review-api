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
using Aide.ClinicalReview.Service.Filter;
using Aide.ClinicalReview.Service.Logging;
using Aide.ClinicalReview.Service.Services;
using Aide.ClinicalReview.Service.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;

namespace Aide.ClinicalReview.Service.Controllers
{
    /// <summary>
    /// ClinicalReview Controller.
    /// </summary>
    [ApiController]
    [Route("clinical-review")]
    public sealed class ClinicalReviewController : ApiControllerBase
    {
        private readonly IClinicalReviewService _clinicalReviewService;
        private readonly ILogger<ClinicalReviewController> _logger;
        private readonly IOptions<AideClinicalReviewServiceOptions> _options;
        private readonly IUriService _uriService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClinicalReviewController"/> class.
        /// </summary>clinicalReviewService">clinical review service to recieve tasks.</param>
        /// <param name="logger">logger.</param>
        public ClinicalReviewController(
            IClinicalReviewService clinicalReviewService,
            IOptions<AideClinicalReviewServiceOptions> options,
            IUriService uriService,
            ILogger<ClinicalReviewController> logger) : base(options)
        {
            _clinicalReviewService = clinicalReviewService ?? throw new ArgumentNullException(nameof(clinicalReviewService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _uriService = uriService ?? throw new ArgumentNullException(nameof(uriService));
        }

        /// <summary>
        /// Gets a paged response list of all clinical review tasks.
        /// </summary>
        /// <param name="filter">Filters.</param>
        /// <param name="patientId">Optional paient Id.</param>
        /// <param name="patientName">Optional patient name.</param>
        /// <param name="applicationName">Optional patient name.</param>
        /// <returns>paged response of subset of all clinical review tasks.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<List<ClinicalReviewRecord>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationFilter filter, [FromQuery] string roles = "", [FromQuery] string? patientId = "", [FromQuery] string? patientName = "",
            [FromQuery] string? applicationName = "")
        {
            try
            {
                var route = Request?.Path.Value ?? string.Empty;
                var pageSize = filter.PageSize ?? 10;
                var validFilter = new PaginationFilter(filter.PageNumber, pageSize, _options.Value.EndpointSettings.MaxPageSize);

                var rolesList = roles.Split(",");

                if (!rolesList.Any() || rolesList.Any(s => string.IsNullOrWhiteSpace(s)))
                {
                    return Problem($"Roles are required.", $"/clinical-review", BadRequest);
                }

                var pagedData = await _clinicalReviewService.GetClinicalReviewListAsync(
                    rolesList,
                    (validFilter.PageNumber - 1) * validFilter.PageSize,
                    validFilter.PageSize,
                    patientId,
                    patientName,
                    applicationName);

                var pagedReponse = CreatePagedReponse(pagedData.ClinicalReviews.ToList(), validFilter, pagedData.recordCount, _uriService, route);

                return Ok(pagedReponse);
            }
            catch (Exception e)
            {
                _logger.ClinicalReviewGetAllAsyncError(e);
                return Problem($"Unexpected error occurred: {e.Message}", $"/clinical-review", InternalServerError);
            }
        }

        /// <summary>
        /// Approve or rejects a clinical review task.
        /// </summary>
        /// <param name="executionId">execution id of the review.</param>
        /// <param name="acknowledge">acknowledgement details.</param>
        /// <returns>204 when updated.</returns>
        [HttpPut]
        [Route("{executionId}")]
        [ProducesResponseType(typeof(StatusCodeResult), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AcknowledgeClinicalReview([FromRoute] string executionId, [FromBody] AcknowledgeClinicalReview acknowledge)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(executionId) || Guid.TryParse(executionId, out var _) is false)
                {
                    return Problem($"Invalid execution id. Must not be null and must be a valid Guid", $"/clinical-review/{executionId}", BadRequest);
                }

                await _clinicalReviewService.AcknowledgeClinicalReview(executionId, acknowledge);

                return new StatusCodeResult(204);
            }
            catch (PreviouslyReviewedException e)
            {
                _logger.ClinicalReviewGetAllAsyncError(e);
                return Problem($"Clinical Review Task Previously Reviewed for executionID: {executionId}", $"/clinical-review/{executionId}", (int)HttpStatusCode.BadRequest);
            }
            catch (UnathorisedRoleException e)
            {
                _logger.ClinicalReviewGetAllAsyncError(e); 
                return Problem($"Unauthorised Roles: {e.Message}", $"/clinical-review/{executionId}", (int)HttpStatusCode.Forbidden);
            }
            catch (MongoNotFoundException e)
            {
                _logger.ClinicalReviewGetAllAsyncError(e);
                return Problem($"No task found for execution ID: {executionId}", $"/clinical-review/{executionId}", NotFound);
            }
            catch (Exception e)
            {
                _logger.ClinicalReviewGetAllAsyncError(e);
                return Problem($"Unexpected error occurred: {e.Message}", $"/clinical-review/{executionId}", InternalServerError);
            }
        }
    }
}
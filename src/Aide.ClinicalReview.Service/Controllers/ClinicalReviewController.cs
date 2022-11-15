using Aide.ClinicalReview.Common.Interfaces;
using Aide.ClinicalReview.Configuration;
using Aide.ClinicalReview.Contracts.Models;
using Aide.ClinicalReview.Service.Filter;
using Aide.ClinicalReview.Service.Logging;
using Aide.ClinicalReview.Service.Services;
using Aide.ClinicalReview.Service.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Aide.ClinicalReview.Service.Controllers
{
    /// <summary>
    /// ClinicalReview Controller.
    /// </summary>
    [ApiController]
    [Route("clinical-review")]
    public class ClinicalReviewController: ApiControllerBase
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
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationFilter filter, [FromQuery] string roles = "", [FromQuery] string? patientId = "", [FromQuery] string? patientName = "", [FromQuery] string? applicationName = "")
        {
            try
            {
                var route = Request?.Path.Value ?? string.Empty;
                var pageSize = filter.PageSize ?? 10;
                var validFilter = new PaginationFilter(filter.PageNumber, pageSize, _options.Value.EndpointSettings.MaxPageSize);

                var rolesList = roles.Split(",");

                if(!rolesList.Any() || rolesList.Any(s => string.IsNullOrWhiteSpace(s)))
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
    }
}

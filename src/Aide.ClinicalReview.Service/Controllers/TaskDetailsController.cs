using Aide.ClinicalReview.Common.Interfaces;
using Aide.ClinicalReview.Configuration;
using Aide.ClinicalReview.Service.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Aide.ClinicalReview.Service.Controllers
{
    [ApiController]
    [Route("task-details")]
    public class TaskDetailsController : ApiControllerBase
    {
        private readonly ITaskDetailsService _taskDetailsService;
        private readonly IOptions<AideClinicalReviewServiceOptions> _options;
        private readonly ILogger<TaskDetailsController> _logger;

        public TaskDetailsController(
            ITaskDetailsService taskDetailsService,
            IOptions<AideClinicalReviewServiceOptions> options,
            ILogger<TaskDetailsController> logger) : base(options)
        {
            _taskDetailsService = taskDetailsService ?? throw new ArgumentNullException(nameof(taskDetailsService));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [Route("{executionId}")]
        public async Task<IActionResult> GetTaskDetailsAsync(Guid executionId)
        {
            try
            {
                var taskDetails = await _taskDetailsService.GetTaskDetailsAsync(executionId);

                if (taskDetails is null)
                {
                    return Problem($"No task found for execution ID: {executionId}", $"/task-details/{executionId}", NotFound);
                }

                return Ok(taskDetails);
            }
            catch (Exception ex)
            {
                _logger.GetTaskDetailsAsyncError(executionId);
                return Problem($"Unexpected error occured: {ex.Message}", $"/task-details/{executionId}", InternalServerError);
            }
        }
    }
}

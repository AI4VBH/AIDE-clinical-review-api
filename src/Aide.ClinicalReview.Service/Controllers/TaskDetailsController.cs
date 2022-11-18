using Aide.ClinicalReview.Common.Interfaces;
using Aide.ClinicalReview.Configuration;
using Aide.ClinicalReview.Contracts.Exceptions;
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
        private readonly ILogger<TaskDetailsController> _logger;

        public TaskDetailsController(
            ITaskDetailsService taskDetailsService,
            IOptions<AideClinicalReviewServiceOptions> options,
            ILogger<TaskDetailsController> logger) : base(options ?? throw new ArgumentNullException(nameof(options)))
        {
            _taskDetailsService = taskDetailsService ?? throw new ArgumentNullException(nameof(taskDetailsService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [Route("{executionId}")]
        public async Task<IActionResult> GetTaskDetailsAsync(Guid executionId,[FromQuery] string roles = "")
        {
            try
            {
                if(executionId == Guid.Empty)
                {
                    return Problem($"execution cannot be null or empty.", $"/task-details/{executionId}", BadRequest);
                }

                var rolesList = roles.Split(",");

                if (!rolesList.Any() || rolesList.Any(s => string.IsNullOrWhiteSpace(s)))
                {
                    return Problem($"Roles are required.", $"/task-details/{executionId}", BadRequest);
                }

                var taskDetails = await _taskDetailsService.GetTaskDetailsAsync(executionId, rolesList);

                return Ok(taskDetails);
            }
            catch (UnathorisedRoleException e)
            {
                _logger.GetTaskDetailsAsyncError(executionId);
                return Problem($"Unauthorised Roles: {e.Message}", $"/task-details/{executionId}", BadRequest);
            }
            catch (MongoNotFoundException e)
            {
                _logger.GetTaskDetailsAsyncError(executionId);
                return Problem($"No task found for execution ID: {executionId}", $"/task-details/{executionId}", NotFound);
            }
            catch (Exception ex)
            {
                _logger.GetTaskDetailsAsyncError(executionId);
                return Problem($"Unexpected error occured: {ex.Message}", $"/task-details/{executionId}", InternalServerError);
            }
        }
    }
}

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
        public async Task<IActionResult> GetTaskDetailsAsync(Guid executionId, [FromQuery] string roles = "")
        {
            try
            {
                if (executionId == Guid.Empty)
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
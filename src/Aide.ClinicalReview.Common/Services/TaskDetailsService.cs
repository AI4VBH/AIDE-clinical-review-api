using Aide.ClinicalReview.Common.Interfaces;
using Aide.ClinicalReview.Contracts.Exceptions;
using Aide.ClinicalReview.Contracts.Models;
using Aide.ClinicalReview.Database.Interfaces;
using Ardalis.GuardClauses;

namespace Aide.ClinicalReview.Common.Services
{
    public class TaskDetailsService : ITaskDetailsService
    {
        private readonly ITaskDetailsRepository _taskDetailsRepository;

        public TaskDetailsService(ITaskDetailsRepository taskDetailsRepository)
        {
            _taskDetailsRepository = taskDetailsRepository;
        }

        public async Task<ClinicalReviewStudy> GetTaskDetailsAsync(Guid executionId, string[] roles)
        {
            Guard.Against.Default(executionId);
            Guard.Against.NullOrEmpty(roles);

            var taskDetails = await _taskDetailsRepository.GetTaskDetailsAsync(executionId);
            if (taskDetails is null)
            {
                throw new MongoNotFoundException($"Task details could not be found for execution ID {executionId}");
            }
            if (taskDetails.Roles.Any(r => roles.Contains(r, StringComparer.InvariantCultureIgnoreCase)) is false) 
            {
                throw new UnathorisedRoleException("Role is unathorised");
            };

            return taskDetails;

        }
    }
}

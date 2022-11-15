using Aide.ClinicalReview.Common.Interfaces;
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

        public async Task<ClinicalReviewStudy> GetTaskDetailsAsync(Guid executionId)
        {
            Guard.Against.Default(executionId);

            return await _taskDetailsRepository.GetTaskDetailsAsync(executionId);
        }
    }
}

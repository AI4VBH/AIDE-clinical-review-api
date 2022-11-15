using Aide.ClinicalReview.Contracts.Models;

namespace Aide.ClinicalReview.Common.Interfaces
{
    public interface ITaskDetailsService
    {
        Task<ClinicalReviewStudy> GetTaskDetailsAsync(Guid executionId);
    }
}

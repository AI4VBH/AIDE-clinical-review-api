using Aide.ClinicalReview.Contracts.Models;

namespace Aide.ClinicalReview.Database.Interfaces
{
    public interface ITaskDetailsRepository
    {
        Task<ClinicalReviewStudy> GetTaskDetailsAsync(Guid executionId);
    }
}

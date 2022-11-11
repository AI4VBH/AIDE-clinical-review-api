using Aide.ClinicalReview.Contracts.Models;

namespace Aide.ClinicalReview.Database.Interfaces
{
    public interface IClinicalReviewRepository
    {
        /// <summary>
        /// Gets a list of the latest ClinicalReviewRecord.
        /// </summary>
        Task<List<ClinicalReviewRecord>> GetClinicalReviewListAsync();

        /// <summary>
        /// Retrieves a ClinicalReviewRecord based on an Id.
        /// </summary>
        /// <param name="clinicalReviewId">The clinicalReview Id.</param>
        Task<ClinicalReviewRecord> GetByClinicalReviewIdAsync(string clinicalReviewId);

        /// <summary>
        /// Creates a ClinicalReviewRecord object.
        /// </summary>
        /// <param name="workflow">Workflow object to create.</param>
        Task<string> CreateAsync(ClinicalReviewRecord clinicalReview);
    }
}
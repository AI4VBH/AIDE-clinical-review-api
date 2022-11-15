using Aide.ClinicalReview.Contracts.Models;

namespace Aide.ClinicalReview.Database.Interfaces
{
    public interface IClinicalReviewRepository
    {
        /// <summary>
        /// Gets a paginated list of the latest ClinicalReviewRecord along with a total count.
        /// </summary>
        Task<(IList<ClinicalReviewRecord> ClinicalReviews, long recordCount)> GetClinicalReviewListAsync(
                                                      string[] roles,
                                                      int? skip = null,
                                                      int? limit = null,
                                                      string? patientId = "",
                                                      string? patientName = "",
                                                      string? applicationName = "");

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
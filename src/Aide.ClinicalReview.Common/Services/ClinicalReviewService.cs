using Aide.ClinicalReview.Common.Interfaces;
using Aide.ClinicalReview.Contracts.Models;
using Aide.ClinicalReview.Database.Interfaces;
using Microsoft.Extensions.Logging;

namespace Aide.ClinicalReview.Common.Services
{
    public sealed class ClinicalReviewService : IClinicalReviewService
    {
        private readonly IClinicalReviewRepository _clinicalReviewRepository;
        private readonly ILogger<ClinicalReviewService> _logger;

        public ClinicalReviewService(IClinicalReviewRepository clinicalReviewRepository, ILogger<ClinicalReviewService> logger)
        {
            _clinicalReviewRepository = clinicalReviewRepository ?? throw new ArgumentNullException(nameof(clinicalReviewRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<(IList<ClinicalReviewRecord> ClinicalReviews, long recordCount)> GetClinicalReviewListAsync(string[] roles,
                                                      int? skip = null,
                                                      int? limit = null,
                                                      string? patientId = "",
                                                      string? patientName = "",
                                                      string? applicationName = "")
            => await _clinicalReviewRepository.GetClinicalReviewListAsync(roles, skip, limit, patientId, patientName, applicationName);
    }
}
using Aide.ClinicalReview.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aide.ClinicalReview.Common.Interfaces
{
    public interface IClinicalReviewService
    {
        Task<(IList<ClinicalReviewRecord> ClinicalReviews, long recordCount)> GetClinicalReviewListAsync(string[] roles,
                                                      int? skip = null,
                                                      int? limit = null,
                                                      string? patientId = "",
                                                      string? patientName = "",
                                                      string? applicationName = "");
    }
}

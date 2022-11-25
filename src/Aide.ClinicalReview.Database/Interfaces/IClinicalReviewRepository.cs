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
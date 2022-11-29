﻿// 
// Copyright 2022 Guy’s and St Thomas’ NHS Foundation Trust
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
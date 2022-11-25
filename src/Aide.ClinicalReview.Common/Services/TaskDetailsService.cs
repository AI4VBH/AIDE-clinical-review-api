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
            }

            ;

            return taskDetails;
        }

        public async Task<string> CreateTaskDetailsAsync(ClinicalReviewStudy clinicalReviewStudy)
        {
            Guard.Against.Null(clinicalReviewStudy);

            return await _taskDetailsRepository.CreateTaskDetailsAsync(clinicalReviewStudy);
        }
    }
}
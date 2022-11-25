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
using Aide.ClinicalReview.Database.Interfaces;
using Aide.ClinicalReview.Database.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Aide.ClinicalReview.Database.Repository
{
    public class TaskDetailsRepository : RepositoryBase, ITaskDetailsRepository
    {
        private readonly IMongoCollection<ClinicalReviewStudy> _clinicalReviewStudyCollection;

        public TaskDetailsRepository(IMongoClient client, IOptions<AideClinicalReviewDatabaseSettings> databaseSettings)
        {
            if (client is null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            var db = client.GetDatabase(databaseSettings.Value.DatabaseName);
            _clinicalReviewStudyCollection = db.GetCollection<ClinicalReviewStudy>(databaseSettings.Value.AideClinicalReviewStudy);
        }

        public async Task<ClinicalReviewStudy> GetTaskDetailsAsync(Guid executionId)
        {
            return await _clinicalReviewStudyCollection
                .Find(x => x.ExecutionId == executionId.ToString())
                .FirstOrDefaultAsync();
        }

        public async Task<string> CreateTaskDetailsAsync(ClinicalReviewStudy clinicalReviewStudy)
        {
            await _clinicalReviewStudyCollection.InsertOneAsync(clinicalReviewStudy);

            return clinicalReviewStudy.ExecutionId;
        }
    }
}
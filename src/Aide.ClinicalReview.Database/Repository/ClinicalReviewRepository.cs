// 
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

using Aide.ClinicalReview.Contracts.Models;
using Aide.ClinicalReview.Database.Configuration;
using Aide.ClinicalReview.Database.Interfaces;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Aide.ClinicalReview.Database.Repository
{
    public sealed class ClinicalReviewRepository : RepositoryBase, IClinicalReviewRepository
    {
        private readonly IMongoCollection<ClinicalReviewRecord> _clinicalReviewCollection;

        public ClinicalReviewRepository(
            IMongoClient client,
            IOptions<AideClinicalReviewDatabaseSettings> databaseSettings)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            var mongoDatabase = client.GetDatabase(databaseSettings.Value.DatabaseName);
            _clinicalReviewCollection = mongoDatabase.GetCollection<ClinicalReviewRecord>(databaseSettings.Value.AideClinicalReviewRecord);
        }

        public async Task<string> CreateAsync(ClinicalReviewRecord clinicalReview)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            clinicalReview.Id = clinicalReview.ClinicalReviewMessage.ExecutionId;
            clinicalReview.Received = DateTime.UtcNow;
            await _clinicalReviewCollection.InsertOneAsync(clinicalReview);
            return clinicalReview.Id;
        }

        public async Task<ClinicalReviewRecord> GetByClinicalReviewIdAsync(string clinicalReviewId)
        {
            Guard.Against.NullOrEmpty(clinicalReviewId);

            var workflow = await _clinicalReviewCollection
                .Find(x => x.Id == clinicalReviewId)
                .FirstOrDefaultAsync();

            return workflow;
        }

        public async Task<(IList<ClinicalReviewRecord> ClinicalReviews, long recordCount)> GetClinicalReviewListAsync(
            string[] roles,
            int? skip = null,
            int? limit = null,
            string? patientId = "",
            string? patientName = "",
            string? applicationName = "")
        {
            var builder = Builders<ClinicalReviewRecord>.Filter;
            var filter = builder.Empty;

            filter &= builder.AnyIn(p => p.ClinicalReviewMessage!.ReviewerRoles, roles);
            if (!string.IsNullOrEmpty(patientId))
            {
                filter &= builder.Regex(p => p.ClinicalReviewMessage!.PatientMetadata!.PatientId, new BsonRegularExpression($"/{patientId}/i"));
            }

            if (!string.IsNullOrEmpty(patientName))
            {
                filter &= builder.Regex(p => p.ClinicalReviewMessage!.PatientMetadata!.PatientName, new BsonRegularExpression($"/{patientName}/i"));
            }

            if (!string.IsNullOrEmpty(applicationName))
            {
                filter &= builder.Regex(p => p.ClinicalReviewMessage!.ApplicationMetadata["application_name"], new BsonRegularExpression($"/{applicationName}/i"));
            }

            var clinicalReviews = await GetAllAsync(_clinicalReviewCollection,
                filter,
                Builders<ClinicalReviewRecord>.Sort.Descending(x => x.Received));

            return (clinicalReviews.Skip((int)skip!).Take((int)limit!).ToList(), clinicalReviews.Count());
        }
    }
}
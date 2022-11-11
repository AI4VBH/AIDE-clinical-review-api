using Aide.ClinicalReview.Contracts.Models;
using Aide.ClinicalReview.Database.Interfaces;
using Aide.ClinicalReview.Database.Options;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Aide.ClinicalReview.Database.Repository
{
    public sealed class ClinicalReviewRepository : IClinicalReviewRepository
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
            _clinicalReviewCollection = mongoDatabase.GetCollection<ClinicalReviewRecord>(databaseSettings.Value.AideClinicalReviewService);
        }

        public async Task<string> CreateAsync(ClinicalReviewRecord clinicalReview)
        {
            clinicalReview.Id = clinicalReview.ClinicalReviewMessage.ExecutionId;
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

        public async Task<List<ClinicalReviewRecord>> GetClinicalReviewListAsync()
        {
            return await _clinicalReviewCollection.AsQueryable().ToListAsync();
        }
    }
}

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
    }
}

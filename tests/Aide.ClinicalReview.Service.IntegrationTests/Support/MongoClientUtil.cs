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
using Aide.ClinicalReview.Service.IntegrationTests.POCO;
using MongoDB.Driver;
using Polly;
using Polly.Retry;

namespace Aide.ClinicalReview.Service.IntegrationTests.Support
{
    public sealed class MongoClientUtil
    {
        private MongoClient Client { get; set; }
        private IMongoDatabase Database { get; set; }
        private IMongoCollection<ClinicalReviewRecord> ClinicalReviewTaskCollection { get; set; }
        private IMongoCollection<ClinicalReviewStudy> ClinicalReviewStudyCollection { get; set; }
        private RetryPolicy RetryMongo { get; set; }

        public MongoClientUtil()
        {
            Client = new MongoClient(TestExecutionConfig.MongoConfig.ConnectionString);
            Database = Client.GetDatabase($"{TestExecutionConfig.MongoConfig.Database}");
            ClinicalReviewTaskCollection = Database.GetCollection<ClinicalReviewRecord>($"{TestExecutionConfig.MongoConfig.AideClinicalReviewRecordCollection}");
            ClinicalReviewStudyCollection = Database.GetCollection<ClinicalReviewStudy>($"{TestExecutionConfig.MongoConfig.AideClinicalReviewStudyCollection}");
            RetryMongo = Policy.Handle<Exception>().WaitAndRetry(retryCount: 10, sleepDurationProvider: _ => TimeSpan.FromMilliseconds(1000));
        }

        #region ClinicalReviewTasks

        public void CreateClinicalReviewTask(ClinicalReviewRecord clinicalReviewRecord)
        {
            RetryMongo.Execute(() => { ClinicalReviewTaskCollection.InsertOne(clinicalReviewRecord); });
        }

        public List<ClinicalReviewRecord> GetClinicalReviewTaskByExecutionId(string executionId)
        {
            return ClinicalReviewTaskCollection.Find(x => x.Id.Equals(executionId)).ToList();
        }

        public void DeleteClinicalReviewTaskByExecutionId(string executionId)
        {
            RetryMongo.Execute(() => { ClinicalReviewTaskCollection.DeleteOne(x => x.Id.Equals(executionId)); });
        }

        public void DeleteAllClinicalReviewTasks()
        {
            RetryMongo.Execute(() =>
            {
                ClinicalReviewTaskCollection.DeleteMany("{ }");

                var clinicalReviewTasks = ClinicalReviewTaskCollection.Find("{ }").ToList();

                if (clinicalReviewTasks.Count > 0)
                {
                    throw new Exception("All Clincial Review Tasks are not deleted!");
                }
            });
        }

        #endregion

        #region ClinicalReviewStudies

        public void CreateClinicalReviewStudy(ClinicalReviewStudy clinicalReviewStudy)
        {
            RetryMongo.Execute(() => { ClinicalReviewStudyCollection.InsertOne(clinicalReviewStudy); });
        }

        public List<ClinicalReviewStudy> GetClinicalReviewStudyByExecutionId(string executionId)
        {
            return ClinicalReviewStudyCollection.Find(x => x.ExecutionId.Equals(executionId)).ToList();
        }

        public void DeleteClinicalReviewStudyByExecutionId(string executionId)
        {
            RetryMongo.Execute(() => { ClinicalReviewStudyCollection.DeleteOne(x => x.ExecutionId.Equals(executionId)); });
        }

        public void DeleteAllClinicalReviewStudies()
        {
            RetryMongo.Execute(() =>
            {
                ClinicalReviewStudyCollection.DeleteMany("{ }");

                var clinicalReviewTasks = ClinicalReviewStudyCollection.Find("{ }").ToList();

                if (clinicalReviewTasks.Count > 0)
                {
                    throw new Exception("All Clincial Review Studies are not deleted!");
                }
            });
        }

        #endregion

        public void DropDatabase(string dbName)
        {
            Client.DropDatabase(dbName);
        }
    }
}
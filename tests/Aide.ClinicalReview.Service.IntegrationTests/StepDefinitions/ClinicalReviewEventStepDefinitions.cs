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
using Aide.ClinicalReview.Service.IntegrationTests.Support;
using MongoDB.Driver;
using Polly;
using Polly.Retry;
using TechTalk.SpecFlow.Infrastructure;

namespace Aide.ClinicalReview.Service.IntegrationTests.StepDefinitions
{
    [Binding]
    public class ClinicalReviewEventStepDefinitions
    {
        // Retry Policy Variables
        private const int RETRY_COUNT = 10;
        private readonly TimeSpan SLEEP_DURATION = TimeSpan.FromMilliseconds(500);

        private DataHelper DataHelper { get; set; }
        private ISpecFlowOutputHelper OutputHelper { get; set; }
        private MinioDataSeeding MinioDataSeeding { get; set; }
        private RetryPolicy RetryPolicy { get; set; }

        public ClinicalReviewEventStepDefinitions(DataHelper dataHelper, MinioDataSeeding minioDataSeeding)
        {
            DataHelper = dataHelper;
            MinioDataSeeding = minioDataSeeding;

            RetryPolicy = Policy.Handle<Exception>()
                .WaitAndRetry(retryCount: RETRY_COUNT, sleepDurationProvider: _ => SLEEP_DURATION);
        }

        [Given(@"I have artifacts in minio (.*)")]
        public async Task GivenIHaveArtifactsInMinio(List<string> paths)
        {
            foreach(var path in paths)
            {
                await MinioDataSeeding.SeedArtifacts("payload", path);
            }
        }

        [When(@"I publish a Clinical Review Request Event (.*)")]
        public void WhenIPublishAClinicalReviewRequestEvent(string name)
        {
            DataHelper.PublishClinicalReviewRequestEvent(name);
        }

        [Then(@"I can see ClinicalReviewRecord in Mongo")]
        public void ThenICanSeeClinicalReviewRecordInMongo()
        {
            RetryPolicy.Execute(() =>
            {
                var actualClinicalReviewTasks = DataHelper.GetClinicalReviewTasksFromEvent();

                if(actualClinicalReviewTasks.Count > 0)
                {
                    Assertions.AssertClinicalReviewTaskFromEvent(actualClinicalReviewTasks, DataHelper.ClinicalReviewEvent);
                }
                else
                {
                    throw new Exception($"Clinical Review record for ExecutionId:{DataHelper.ClinicalReviewEvent.ExecutionId} canot be found");
                }
            });
        }

        [Then(@"I can see StudyRecord in Mongo matches (.*)")]
        public void ThenICanSeeStudyRecordInMongoMatches(string name)
        {
            var expectedClinicalReviewStudy = DataHelper.GetClinicalReviewStudy(name);

            var actualClinicalReviewStudies = new List<ClinicalReviewStudy>();

            RetryPolicy.Execute(() =>
            {
               actualClinicalReviewStudies = DataHelper.GetClinicalReviewStudiesFromEvent();

                if(actualClinicalReviewStudies.Count <= 0)
                {
                    throw new Exception($"Clinical Review studies for ExecutionId:{DataHelper.ClinicalReviewEvent.ExecutionId} canot be found");
                }
            });

            Assertions.AssertClinicalReviewStudyFromEvent(actualClinicalReviewStudies, DataHelper.ClinicalReviewEvent, expectedClinicalReviewStudy);
        }

        [Then(@"I can see the correct roles are applied")]
        public void ThenICanSeeTheCorrectRolesAreApplied()
        {
            var actualClinicalReviewTasks = new List<ClinicalReviewRecord>();
            var actualClinicalReviewStudies = new List<ClinicalReviewStudy>();

            RetryPolicy.Execute(() =>
            {
                actualClinicalReviewStudies = DataHelper.GetClinicalReviewStudiesFromEvent();

                actualClinicalReviewTasks = DataHelper.GetClinicalReviewTasksFromEvent();

                if (actualClinicalReviewStudies.Count <= 0 && actualClinicalReviewTasks.Count <= 0)
                {
                    throw new Exception($"Clinical Review tasks for ExecutionId:{DataHelper.ClinicalReviewEvent.ExecutionId} canot be found");
                }
            });

            Assertions.AssertClinicalReviewRolesFromEvent(actualClinicalReviewTasks, actualClinicalReviewStudies, DataHelper.ClinicalReviewEvent);
        }


        [StepArgumentTransformation]
        public List<string> TransformToListOfString(string list)
        {
            return list.Split(",").Select(x => x.Trim()).ToList();
        }
    }
}
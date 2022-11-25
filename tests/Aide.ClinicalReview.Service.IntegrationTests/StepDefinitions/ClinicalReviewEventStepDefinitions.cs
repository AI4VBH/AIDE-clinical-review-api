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

using Aide.ClinicalReview.Service.IntegrationTests.Support;

namespace Aide.ClinicalReview.Service.IntegrationTests.StepDefinitions
{
    [Binding]
    public class ClinicalReviewEventStepDefinitions
    {
        DataHelper DataHelper { get; set; }

        public ClinicalReviewEventStepDefinitions(DataHelper dataHelper)
        {
            DataHelper = dataHelper;
        }

        [When(@"I publish a Clinical Review Event (.*)")]
        public void GivenIPublishAClinicalReviewEvent(string name)
        {
            DataHelper.PublishClinicalReviewRequestEvent(name);
        }

        [Then(@"I can see Clinical Review Task is saved in Mongo")]
        public void ThenICanSeeClinicalReviewTaskIsSavedInMongo()
        {
            var clinicalReviewTasks = DataHelper.GetClinicalReviewTasksFromEvent();

            Assertions.AssertClinicalReviewTaskFromEvent(clinicalReviewTasks, DataHelper.ClinicalReviewEvent);
        }

        [Then(@"I can see Clinical Review Study is saved in Mongo")]
        public void ThenICanSeeClinicalReviewStudyIsSavedInMongo()
        {
            var clinicalReviewStudies = DataHelper.GetClinicalReviewStudiesFromEvent();

            Assertions.AssertClinicalReviewStudyFromEvent(clinicalReviewStudies, DataHelper.ClinicalReviewEvent);
        }
    }
}
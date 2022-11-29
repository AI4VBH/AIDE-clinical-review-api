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
using Aide.ClinicalReview.Service.IntegrationTests.Support;
using System.Text.Json;

namespace Aide.ClinicalReview.Service.IntegrationTests.StepDefinitions
{
    [Binding]
    public class ClinicalReviewApiStepDefinitions
    {
        private DataHelper DataHelper { get; set; }
        private HttpResponseMessage HttpResponse { get; set; }
        private string Action;

        public ClinicalReviewApiStepDefinitions(DataHelper dataHelper)
        {
            DataHelper = dataHelper;
        }

        [When(@"I (.*) the Clinical Review Task (.*)")]
        public async Task WhenIActionTheClinicalReviewTask(string action, string clinicalReviewTask)
        {
            Action = action;
            DataHelper.CreateClinicalReviewTask(clinicalReviewTask);
            await DataHelper.SendClinicalReviewRequest(action);
        }

        [Then(@"I can see Clinical Review Task is updated")]
        public async Task ThenICanSeeClinicalReviewTaskIsUpdated()
        {
            HttpResponse = await DataHelper.GetClinicalReviewTasks();
            var response = HttpResponse.Content.ReadAsStringAsync().Result;
            var ClinicalReviewTasks = JsonSerializer.Deserialize<List<ClinicalReviewRecord>>(response);

            Assertions.AssertClinicalReviewTaskStatusUpdated(ClinicalReviewTasks, Action);
        }

        [Then(@"I can see a Task Callback is generated")]
        public void ThenICanSeeATaskCallbackIsGenerated()
        {
            var taskCallbackEvent = DataHelper.GetTaskCallbackEvent();

            Assertions.AssertTaskCallbackEvent();
        }
    }
}
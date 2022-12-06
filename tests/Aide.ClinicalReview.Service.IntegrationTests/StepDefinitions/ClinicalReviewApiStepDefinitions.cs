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
using Aide.ClinicalReview.Service.IntegrationTests.POCO;
using Aide.ClinicalReview.Service.IntegrationTests.Support;
using Aide.ClinicalReview.Service.Wrappers;
using Monai.Deploy.Messaging.Events;
using Newtonsoft.Json;
using System.Net;
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

        [When(@"I send a request to edit clinical review task with '(.*)' and execution Id '(.*)'")] 
        public async Task WhenIActionTheClinicalReviewTask(string body, string executionId)
        {
            var test = await DataHelper.EditClinicalReviewRequest(body, executionId);
        }

        [Then(@"clinical review task has been updated in Mongo '(.*)'")]
        public void ThenClinicalReviewTaskHasBeenUpdatedInMongo(string executionId)
        {
            var result =  DataHelper.GetClinicalReviewTask(executionId);
        }


        [Then(@"I can see a Task Callback is generated")]
        public void ThenICanSeeATaskCallbackIsGenerated()
        {
            var message = DataHelper.GetTaskCallbackEvent();
        }
    }
}
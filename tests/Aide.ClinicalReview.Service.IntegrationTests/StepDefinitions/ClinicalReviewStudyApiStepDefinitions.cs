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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Aide.ClinicalReview.Service.IntegrationTests.StepDefinitions
{
    [Binding]
    public class ClinicalReviewStudyApiStepDefinitions
    {
        private DataHelper DataHelper { get; set; }
        private HttpResponseMessage HttpResponse { get; set; }

        public ClinicalReviewStudyApiStepDefinitions(DataHelper dataHelper)
        {
            DataHelper = dataHelper;
        }

        [Given(@"I have Clinical Review studies '(.*)' in Mongo")]
        public void GivenIHaveClinicalReviewStudiesInMongo(List<string> names)
        {
            foreach (var name in names)
            {
                DataHelper.CreateClinicalReviewStudy(name);
            }
        }

        [When(@"I send a request to get Clinical Review Studies '(.*)'")]
        public async Task WhenISendARequestToGetClinicalReviewStudies(string executionId)
        {
            HttpResponse = await DataHelper.GetClinicalReviewStudies(executionId);
        }

        [Then(@"I can see correct Studies are returned")]
        public void ThenICanSeeCorrectStudiesAreReturned()
        {
            HttpResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var response = (HttpResponse.Content.ReadAsStringAsync().Result);

            var actualClinicalReviewStudies = JsonConvert.DeserializeObject<ClinicalReviewStudy>(response);

            Assertions.AssertClinicalReviewStudies(actualClinicalReviewStudies, DataHelper.ClinicalReviewStudies);
        }

        [Given(@"I have no Clinical Review Study in Mongo")]
        public void GivenIHaveNoClinicalReviewStudyInMongo()
        {
            // for BDD readability only
        }

        [Then(@"No Clinical Review Study is returned")]
        public void ThenICanSeeNoClinicalReviewStudyIsReturned()
        {
            HttpResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var response = JObject.Parse(HttpResponse.Content.ReadAsStringAsync().Result);

            var actualClinicalReviewTasks = JsonConvert.DeserializeObject<ClinicalReviewStudy>(response["data"].ToString());

            actualClinicalReviewTasks.Should().Be(0);
        }

        [Then(@"Clinical Review Study Returns Bad request")]
        public void ThenICanClinicalReviewServiceReturnsBadRequest()
        {
            HttpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Then(@"Clinical Review Study Returns Not found")]
        public void ThenICanClinicalReviewServiceReturnsNotFound()
        {
            HttpResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [StepArgumentTransformation]
        public List<string> TransformToListOfString(string list)
        {
            return list.Split(",").ToList();
        }
    }
}
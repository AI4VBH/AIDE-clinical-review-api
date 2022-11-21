using Aide.ClinicalReview.Contracts.Models;
using Aide.ClinicalReview.Service.IntegrationTests.Support;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Text.Json;
using TechTalk.SpecFlow;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

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

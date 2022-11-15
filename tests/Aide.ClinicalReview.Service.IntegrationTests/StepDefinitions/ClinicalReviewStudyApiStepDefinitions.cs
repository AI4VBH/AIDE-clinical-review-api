using Aide.ClinicalReview.Contracts.Models;
using Aide.ClinicalReview.Service.IntegrationTests.Support;
using System;
using System.Text.Json;
using TechTalk.SpecFlow;

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

        [When(@"I send a request to get Clinical Review Studies")]
        public async Task WhenISendARequestToGetClinicalReviewStudies()
        {
            HttpResponse = await DataHelper.GetClinicalReviewStudies();
        }

        [Then(@"I can see correct Studies are returned")]
        public void ThenICanSeeCorrectStudiesAreReturned()
        {
            var response = HttpResponse.Content.ReadAsStringAsync().Result;

            var actualClinicalReviewStudies = JsonSerializer.Deserialize<List<ClinicalReviewStudy>>(response);

            Assertions.AssertClinicalReviewStudies(actualClinicalReviewStudies, DataHelper.ClinicalReviewStudies);
        }

        [StepArgumentTransformation]
        public List<string> TransformToListOfString(string list)
        {
            return list.Split(",").ToList();
        }
    }
}

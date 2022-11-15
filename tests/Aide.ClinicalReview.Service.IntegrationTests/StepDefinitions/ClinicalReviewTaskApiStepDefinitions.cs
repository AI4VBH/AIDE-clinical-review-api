using Aide.ClinicalReview.Contracts.Models;
using Aide.ClinicalReview.Service.IntegrationTests.Support;
using System.Text.Json;

namespace Aide.ClinicalReview.Service.IntegrationTests.StepDefinitions
{
    [Binding]
    public class ClinicalReviewTaskApiStepDefinitions
    {
        private DataHelper DataHelper { get; set; }
        private HttpResponseMessage HttpResponse { get; set; }

        public ClinicalReviewTaskApiStepDefinitions(DataHelper dataHelper)
        {
            DataHelper = dataHelper;
        }

        [Given(@"I have Clinical Review Tasks '(.*)' in Mongo")]
        public void GivenIHaveClinicalReviewTasksInMongo(List<string> names)
        {
            foreach(var name in names)
            {
                DataHelper.CreateClinicalReviewTask(name);
            }
        }

        [When(@"I send a request to get Clinical Review Tasks")]
        public async Task WhenISendARequestToGetClinicalReviewTasks()
        {
            HttpResponse = await DataHelper.GetClinicalReviewTasks();
        }

        [Then(@"I can see correct Tasks are returned")]
        public void ThenICanSeeCorrectTasksAreReturned()
        {
            var response = HttpResponse.Content.ReadAsStringAsync().Result;

            var actualClinicalReviewTasks = JsonSerializer.Deserialize<List<ClinicalReviewRecord>>(response);

            Assertions.AssertClinicalReviewTasks(actualClinicalReviewTasks, DataHelper.ClinicalReviewTasks);
        }

        [StepArgumentTransformation]
        public List<string> TransformToListOfString(string list)
        {
            return list.Split(",").ToList();
        }
    }
}

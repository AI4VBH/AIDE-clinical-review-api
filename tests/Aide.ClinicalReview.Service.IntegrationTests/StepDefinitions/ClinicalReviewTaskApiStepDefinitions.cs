using Aide.ClinicalReview.Contracts.Models;
using Aide.ClinicalReview.Service.IntegrationTests.Models;
using Aide.ClinicalReview.Service.IntegrationTests.Support;
using Aide.ClinicalReview.Service.Wrappers;
using Newtonsoft.Json.Linq;
using System.Net;
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

        [Given(@"I have no Clinical Review Tasks in Mongo")]
        public void GivenIHaveNoClinicalReviewTasksInMongo()
        {
            // for BDD readability only
        }

        [When(@"I send a request to get Clinical Review Tasks")]
        public async Task WhenISendARequestToGetClinicalReviewTasks()
        {
            var parameters = new Dictionary<string, string>()
            {
                { "roles", "clinician" } 
            };

            HttpResponse = await DataHelper.GetClinicalReviewTasks(parameters);
        }

        [When(@"I send a request to get Clinical Review Tasks with parameter (.*) and (.*)")]
        public async Task WhenISendARequestToGetClinicalReviewTasksWithParameters(string name, string value)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            if(name != "roles")
            {
                parameters["roles"] = "clinician";
                parameters[name] = value;
            }
            else
            {
                parameters[name] = value;
            }

            HttpResponse = await DataHelper.GetClinicalReviewTasks(parameters);
        }


        [Then(@"I can see correct Clinical Review Tasks are returned")]
        public void ThenICanSeeCorrectClinicalReviewTasksAreReturned()
        {
            HttpResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var response = JObject.Parse(HttpResponse.Content.ReadAsStringAsync().Result);

            var actualClinicalReviewTasks = JsonSerializer.Deserialize<List<ClinicalReviewRecord>>(response["data"].ToString());

            Assertions.AssertClinicalReviewTasks(actualClinicalReviewTasks, DataHelper.ClinicalReviewTasks);
        }

        [Then(@"I can see no Clinical Review Tasks are returned")]
        public void ThenICanSeeNoClinicalReviewTasksAreReturned()
        {
            HttpResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var response = JObject.Parse(HttpResponse.Content.ReadAsStringAsync().Result);

            var actualClinicalReviewTasks = JsonSerializer.Deserialize<List<ClinicalReviewRecord>>(response["data"].ToString());

            actualClinicalReviewTasks.Count.Should().Be(0);
        }


        [StepArgumentTransformation]
        public List<string> TransformToListOfString(string list)
        {
            return list.Split(",").ToList();
        }
    }
}

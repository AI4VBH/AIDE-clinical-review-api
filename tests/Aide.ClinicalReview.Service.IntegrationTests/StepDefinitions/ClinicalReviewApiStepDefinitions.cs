using Aide.ClinicalReview.Contracts.Models;
using Aide.ClinicalReview.Service.IntegrationTests.Support;
using Microsoft.AspNetCore.Http;
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

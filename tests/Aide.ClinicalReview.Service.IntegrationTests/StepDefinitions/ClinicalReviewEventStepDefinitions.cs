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

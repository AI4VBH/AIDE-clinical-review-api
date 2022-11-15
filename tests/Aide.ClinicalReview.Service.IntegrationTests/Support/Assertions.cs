using Aide.ClinicalReview.Contracts.Messages;
using Aide.ClinicalReview.Contracts.Models;

namespace Aide.ClinicalReview.Service.IntegrationTests.Support
{
    public static class Assertions
    {
        public static void AssertClinicalReviewTasks(List<ClinicalReviewRecord>? actualClinicalReviewTasks, List<ClinicalReviewRecord>? expectedClinicalReviewTasks)
        {
            actualClinicalReviewTasks?.Count.Should().Be(expectedClinicalReviewTasks?.Count);
            actualClinicalReviewTasks?.Select(x => x.Id).ToArray().Should().BeEquivalentTo(expectedClinicalReviewTasks?.Select(x => x.Id).ToArray());
        }

        public static void AssertClinicalReviewTaskStatusUpdated(List<ClinicalReviewRecord>? clinicalReviewTasks, string action)
        {
            foreach(var clinicalReviewTask in clinicalReviewTasks)
            {
                clinicalReviewTask.Reviewed.Should().Be(action); // may need updating
            }
        }

        public static void AssertClinicalReviewTaskFromEvent(List<ClinicalReviewRecord> clinicalReviewTasks, AideClinicalReviewRequestMessage clinicalReviewEvent)
        {
            foreach (var clinicalReviewTask in clinicalReviewTasks)
            {
                // assertions
            }
        }

        internal static void AssertClinicalReviewStudyFromEvent(List<ClinicalReviewStudy> clinicalReviewStudies, AideClinicalReviewRequestMessage clinicalReviewEvent)
        {
            foreach (var clinicalReviewStudy in clinicalReviewStudies)
            {
                // assertions
            }
        }

        public static void AssertTaskCallbackEvent()
        {
            // assertions
        }

        internal static void AssertClinicalReviewStudies(List<ClinicalReviewStudy>? actualClinicalReviewStudies, List<ClinicalReviewStudy>? expectedClinicalReviewStudies)
        {
            actualClinicalReviewStudies?.Count.Should().Be(expectedClinicalReviewStudies?.Count);
            actualClinicalReviewStudies?.Select(x => x.ExecutionId ).ToArray().Should().BeEquivalentTo(expectedClinicalReviewStudies?.Select(x => x.ExecutionId).ToArray());
        }
    }
}

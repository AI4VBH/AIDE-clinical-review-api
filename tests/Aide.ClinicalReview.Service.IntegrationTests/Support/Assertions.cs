// 
// Copyright 2022 Crown Copyright
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
            foreach (var clinicalReviewTask in clinicalReviewTasks)
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

        internal static void AssertClinicalReviewStudies(ClinicalReviewStudy actualClinicalReviewStudies, ClinicalReviewStudy expectedClinicalReviewStudies)
        {
            actualClinicalReviewStudies.Should().BeEquivalentTo(expectedClinicalReviewStudies);
        }
    }
}
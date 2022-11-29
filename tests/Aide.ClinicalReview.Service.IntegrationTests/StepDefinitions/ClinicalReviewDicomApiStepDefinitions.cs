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

using Aide.ClinicalReview.Service.IntegrationTests.Support;
using System.Net;

namespace Aide.ClinicalReview.Service.IntegrationTests.StepDefinitions
{
    [Binding]
    public class ClinicalReviewDicomApiStepDefinitions
    {
        DataHelper DataHelper { get; set; }
        MinioDataSeeding MinioDataSeeding { get; set; }

        private HttpResponseMessage HttpResponse { get; set; }

        public ClinicalReviewDicomApiStepDefinitions(DataHelper dataHelper, MinioDataSeeding minioDataSeeding)
        {
            DataHelper = dataHelper;
            MinioDataSeeding = minioDataSeeding;
        }

        [Given(@"I have Dicom files in Minio")]
        public async Task GivenIHaveDicomFilesInMinio()
        {
            await MinioDataSeeding.SeedArtifacts("payload", "study/dcm/series/");
            await MinioDataSeeding.SeedArtifacts("payload", "study/workflows/task1/execution1/");
        }

        [When(@"I send a request to get Dicom file (.*)")]
        public async Task WhenISendARequestToGetDicomFile(string file)
        {
            HttpResponse = await DataHelper.GetClinicalReviewDicoms(file);
        }

        [Then(@"I can see correct Dicom file is returned")]
        public void ThenICanSeeCorrectDicomFileIsReturned()
        {
            HttpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            HttpResponse.Content.Should().BeOfType<StreamContent>();

            var streamContent = HttpResponse.Content as StreamContent;
        }

        [Then(@"I receive a not found response")]
        public void ThenIReceiveANotFoundResponse()
        {
            HttpResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
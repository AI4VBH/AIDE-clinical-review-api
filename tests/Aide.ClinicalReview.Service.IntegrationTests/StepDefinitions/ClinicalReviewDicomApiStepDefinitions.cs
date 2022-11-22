using Aide.ClinicalReview.Service.IntegrationTests.Support;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using TechTalk.SpecFlow;

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

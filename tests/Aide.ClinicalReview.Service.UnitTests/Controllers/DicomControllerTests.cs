using Aide.ClinicalReview.Configuration;
using Aide.ClinicalReview.Service.Controllers;
using Aide.ClinicalReview.Service.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Monai.Deploy.Storage.API;
using Monai.Deploy.Storage.Configuration;
using Moq;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aide.ClinicalReview.Service.UnitTests.Controllers
{
    public sealed class DicomControllerTests
    {
        private IOptions<AideClinicalReviewServiceOptions> _options;
        private Mock<ILogger<DicomController>> _logger;
        private Mock<IDicomService> _dicomService;

        public DicomController DicomController { get; }

        public DicomControllerTests()
        {
            _logger = new Mock<ILogger<DicomController>>();
            _options = Options.Create(new AideClinicalReviewServiceOptions());
            _dicomService = new Mock<IDicomService>(); 

            DicomController = new DicomController(_options, _logger.Object, _dicomService.Object);
        }

        [Fact]
        public async Task Get_WithValidKey_ReturnsOk()
        {
            var key = "bucket1/0b273c5b-4d9c-4521-84c4-72382013f476/dcmoutput.dcm";
            var expectedContentType = "application/dicom";
            using var test_Stream = new MemoryStream(Encoding.UTF8.GetBytes("whatever"));

            var ct = new CancellationTokenSource();
            _dicomService.Setup(s => s.GetDicomFileAsync(It.Is<string>(i => i.Equals(key))))
                .ReturnsAsync(test_Stream);

            var result = await DicomController.GetDicomFile(key);

            Assert.NotNull(result);
            var objectResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal(expectedContentType, objectResult.ContentType);
        }

        [Fact]
        public async Task Get_WithValidKey_ReturnsNotFound()
        {
            var key = "bucket1/0b273c5b-4d9c-4521-84c4-72382013f476/dcmoutput.dcm";

            var result = await DicomController.GetDicomFile(key);

            Assert.NotNull(result);
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, objectResult.StatusCode);
            var responseValue = (ProblemDetails)objectResult!.Value!;
            string expectedErrorMessage = $"Request failed, no dicom file found for key: bucket1/0b273c5b-4d9c-4521-84c4-72382013f476/dcmoutput.dcm";
            responseValue.Detail.Should().BeEquivalentTo(expectedErrorMessage);
        }

        [Fact]
        public async Task Get_WithNoKey_NotFound()
        {
            var key = string.Empty;

            var result = await DicomController.GetDicomFile(key);

            Assert.NotNull(result);
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
            var responseValue = (ProblemDetails)objectResult!.Value!;
            string expectedErrorMessage = $"Failed to validate key, missing key";
            responseValue.Detail.Should().BeEquivalentTo(expectedErrorMessage);
        }

    }
}

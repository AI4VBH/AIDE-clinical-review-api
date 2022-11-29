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

using Aide.ClinicalReview.Configuration;
using Aide.ClinicalReview.Service.Controllers;
using Aide.ClinicalReview.Service.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio.DataModel;
using Monai.Deploy.Storage.API;
using Monai.Deploy.Storage.Configuration;
using Moq;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aide.ClinicalReview.Service.UnitTests.Controllers
{
    public sealed class DicomServicesTests
    {
        private IOptions<StorageServiceConfiguration> _options;
        private Mock<IStorageService> _storageService;
        private Mock<ILogger<DicomService>> _logger;

        public DicomService DicomServiceObject { get; }

        public DicomServicesTests()
        {
            _storageService = new();
            _logger = new();
            _options = Options.Create(new StorageServiceConfiguration());

            DicomServiceObject = new DicomService(_storageService.Object, _logger.Object, _options);
        }

        [Theory]
        [InlineData("bucket1", "0b273c5b-4d9c-4521-84c4-72382013f476/dcmoutput.dcm")]
        public async Task GetDicomFileAsync_WithValidKey_ReturnsStream(string bucket, string key)
        {
            _options.Value.Settings.Add(StorageConfiguration.Bucket, bucket);
            using var test_Stream = new MemoryStream(Encoding.UTF8.GetBytes("whatever"));

            _storageService.Setup(s => s.GetObjectAsync(
                    It.Is<string>(i => i.Equals(bucket)),
                    It.Is<string>(i => i.Equals(key)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(test_Stream);

            var result = await DicomServiceObject.GetDicomFileAsync(key);

            Assert.NotNull(result);
            result!.Equals(test_Stream);
        }

        [Theory]
        [InlineData("bucket1", "/0b273c5b-4d9c-4521-84c4-72382013f476/dcmoutput.dcm")]
        public async Task GetDicomFileAsync_WithValidKey_ReturnsNull(string bucket, string key)
        {
            _options.Value.Settings.Add(StorageConfiguration.Bucket, bucket);
            using var test_Stream = new MemoryStream(Encoding.UTF8.GetBytes("whatever"));

            _storageService.Setup(s => s.GetObjectAsync(
                    It.Is<string>(i => i.Equals(bucket)),
                    It.Is<string>(i => i.Equals(key)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Stream)null);

            var result = await DicomServiceObject.GetDicomFileAsync(key);

            Assert.Null(result);
        }
    }
}
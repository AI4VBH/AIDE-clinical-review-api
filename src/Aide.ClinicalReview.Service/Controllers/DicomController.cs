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
using Aide.ClinicalReview.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Aide.ClinicalReview.Service.Controllers
{
    [ApiController]
    [Route("dicom")]
    public sealed class DicomController : ApiControllerBase
    {
        public const string ENDPOINT = "dicom";

        private readonly ILogger<DicomController> _logger;
        private readonly IDicomService _dicomService;

        public DicomController(
            IOptions<AideClinicalReviewServiceOptions> options,
            ILogger<DicomController> logger,
            IDicomService dicomService)
            : base(options)
        {
            _logger = logger;
            _dicomService = dicomService;
        }

        /// <summary>
        /// Gets dicom image.
        /// </summary>
        /// <param name="key">Dicom key.</param>
        /// <returns>Dicom file in format application/dicom.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDicomFile([FromQuery] string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                _logger.LogDebug($"{nameof(GetDicomFile)} - Failed to validate {nameof(key)}");

                return Problem($"Failed to validate {nameof(key)}, missing key", $"{ENDPOINT}", BadRequest);
            }

            var dicom = await _dicomService.GetDicomFileAsync(key);
            if (dicom == null)
            {
                return Problem($"Request failed, no dicom file found for key: {key}", $"{ENDPOINT}", NotFound);
            }

            var keyValues = key.Replace('\\', '/').Split('/');

            return new FileStreamResult(dicom, "application/dicom")
            {
                FileDownloadName = keyValues.LastOrDefault() ?? "unnamed-dicom.dcm"
            };
        }
    }
}
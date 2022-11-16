using Aide.ClinicalReview.Configuration;
using Aide.ClinicalReview.Service.Logging;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Monai.Deploy.Storage.API;
using Monai.Deploy.Storage.Configuration;

namespace Aide.ClinicalReview.Service.Services
{
    public sealed class DicomService : IDicomService
    {
        private readonly IStorageService _storageService;
        private readonly ILogger<DicomService> _logger;
        private readonly IOptions<StorageServiceConfiguration> _options;

        public DicomService(
            IStorageService storageService,
            ILogger<DicomService> logger,
            IOptions<StorageServiceConfiguration> options)
        {
            _storageService = storageService;
            _logger = logger;
            _options = options;
        }

        public async Task<Stream?> GetDicomFileAsync(string key)
        {
            Guard.Against.NullOrWhiteSpace(key);

            var keyValues = key.Replace('\\', '/').Split('/');
            var bucket = keyValues.First();
            if (string.IsNullOrWhiteSpace(bucket))
            {
                bucket = _options.Value.Settings[StorageConfiguration.Bucket];
            }
            if (key.StartsWith(bucket))
            {
                key = string.Join('/', keyValues.Skip(1));
            }

            try
            {
                return await _storageService.GetObjectAsync(bucket, key);
            }
            catch (Exception ex)
            {
                _logger.DicomException(ex.Message, ex);
                return null;
            }
        }
    }
}

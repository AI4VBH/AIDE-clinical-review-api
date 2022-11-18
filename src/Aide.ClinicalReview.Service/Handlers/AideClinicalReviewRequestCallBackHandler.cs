using Aide.ClinicalReview.Contracts.Messages;
using File = Aide.ClinicalReview.Contracts.Messages.File;
using Aide.ClinicalReview.Contracts.Models;
using Aide.ClinicalReview.Database.Interfaces;
using Ardalis.GuardClauses;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Monai.Deploy.Messaging.Messages;
using Aide.ClinicalReview.Service.Services;
using Aide.ClinicalReview.Service.Extensions;
using FellowOakDicom;

namespace Aide.ClinicalReview.Service.Handler
{
    public sealed class ReviewRequestCallBackHandler : ICallBackHandler<AideClinicalReviewRequestMessage>
    {
        private readonly IServiceScope _scope;
        private readonly ILogger<ReviewRequestCallBackHandler> _logger;
        private readonly IClinicalReviewRepository _clinicalReviewRepository;
        private readonly ITaskDetailsRepository _taskDetailsRepository;
        private readonly IDicomService _dicomService;

        public ReviewRequestCallBackHandler(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<ReviewRequestCallBackHandler> logger,
            IClinicalReviewRepository clinicalReviewRepository,
            ITaskDetailsRepository taskDetailsRepository,
            IDicomService dicomService)
        {
            Guard.Against.Null(serviceScopeFactory);
            Guard.Against.Null(logger);
            Guard.Against.Null(clinicalReviewRepository);
            Guard.Against.Null(taskDetailsRepository);
            Guard.Against.Null(dicomService);

            _scope = serviceScopeFactory.CreateScope();
            _logger = logger;
            _clinicalReviewRepository = clinicalReviewRepository;
            _taskDetailsRepository = taskDetailsRepository;
            _dicomService = dicomService;
        }

        public async Task HandleMessage(JsonMessage<AideClinicalReviewRequestMessage> message)
        {
            Guard.Against.Null(message);

            message.Body.Validate();

            var readyState = "false";
            var reviewed = "false";

            var clinicalReviewRecord = new ClinicalReviewRecord()
            {
                ClinicalReviewMessage = message.Body,
                Ready = readyState, 
                Reviewed = reviewed
            };

            var executionId = await _clinicalReviewRepository.CreateAsync(clinicalReviewRecord);

            await HandleTaskDetails(executionId, message.Body.Files);
        }

        private async Task HandleTaskDetails(string executionId, List<File> files)
        {
            Guard.Against.NullOrWhiteSpace(executionId);

            var clinicalReviewStudy = new ClinicalReviewStudy()
            {
                ExecutionId = executionId,
            };

            foreach (var file in files)
            {
                var dcmFilesInfo = await _dicomService.GetAllDicomFileInfoInPath(file.RelativeRootPath, file.Bucket);

                if (dcmFilesInfo.IsNullOrEmpty())
                {
                    continue; // should I be throwing something here or just skip?
                }

                var series = new Series();
                var filesDictionary = new SortedDictionary<int, string>();

                foreach (var fileInfo in dcmFilesInfo)
                {
                    // do I need to do a check here that the current file is a .dcm file?
                    var dcmFileStream = await _dicomService.GetDicomFileAsync(fileInfo.FilePath, file.Bucket);

                    if (dcmFileStream is null)
                    {
                        continue; // not sure if this is the correct thing to be doing here
                    }

                    var dcmFile = await DicomFile.OpenAsync(dcmFileStream, FileReadOption.Default);

                    // Study properties
                    if (string.IsNullOrWhiteSpace(clinicalReviewStudy.StudyUid))
                    {
                        clinicalReviewStudy.StudyUid = GetValueFromDicomTag<string>(dcmFile, DicomTag.StudyInstanceUID);
                    }

                    if (clinicalReviewStudy.StudyDate == null)
                    {
                        clinicalReviewStudy.StudyDate = GetValueFromDicomTag<DateTime>(dcmFile, DicomTag.StudyDate);
                    }

                    if (string.IsNullOrWhiteSpace(clinicalReviewStudy.StudyDescription))
                    {
                        clinicalReviewStudy.StudyDescription = GetValueFromDicomTag<string>(dcmFile, DicomTag.StudyDescription);
                    }

                    // Series properties
                    if (string.IsNullOrWhiteSpace(series.Id))
                    {
                        series.Id = GetValueFromDicomTag<string>(dcmFile, DicomTag.SeriesInstanceUID);
                    }

                    if (string.IsNullOrWhiteSpace(series.Modality))
                    {
                        series.Modality = GetValueFromDicomTag<string>(dcmFile, DicomTag.Modality);
                    }

                    filesDictionary.Add(
                        GetValueFromDicomTag<int>(dcmFile, DicomTag.InstanceNumber),
                        fileInfo.FilePath
                    );
                }

                series.Files = filesDictionary.Values.ToList();

                clinicalReviewStudy.Study.Add(series);

                // TODO
                // Set the roles for the clinical review study
                // Not sure exactly where these come from? Maybe sent as part of the message?
                // Or will they need to be pulled from the auth?
            }

            await _taskDetailsRepository.CreateTaskDetailsAsync(clinicalReviewStudy);
        }

        private T? GetValueFromDicomTag<T>(DicomFile dcmFile, DicomTag tag)
        {
            var receivedValue = dcmFile.Dataset.TryGetSingleValue<T>(tag, out T value);
            if (receivedValue)
            {
                return value;
            }

            return default(T);
        }
    }
}
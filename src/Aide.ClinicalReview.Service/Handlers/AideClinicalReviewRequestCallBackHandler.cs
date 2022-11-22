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
using Aide.ClinicalReview.Common.Constants;

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

            await _clinicalReviewRepository.CreateAsync(clinicalReviewRecord);

            await HandleTaskDetails(message.Body.ExecutionId, message.Body.Files, message.Body.ReviewerRoles.ToList());
        }

        private async Task HandleTaskDetails(string executionId, List<File> files, List<string> roles)
        {
            Guard.Against.NullOrWhiteSpace(executionId);

            if (roles.IsNullOrEmpty())
            {
                roles = new List<string> { RoleConstants.Clinician };
            }

            var clinicalReviewStudy = new ClinicalReviewStudy()
            {
                ExecutionId = executionId,
                Roles = roles
            };

            foreach (var file in files)
            {
                var dcmFilesInfo = await _dicomService.GetAllDicomFileInfoInPath(file.RelativeRootPath, file.Bucket);

                // TODO
                // Clarify how to handle this case
                // May need to incorporate some sort of callback to inform failure

                //if (dcmFilesInfo.IsNullOrEmpty())
                //{
                //    continue; // should I be throwing something here or just skip?
                //}

                var series = new Series();
                var filesDictionary = new SortedDictionary<int, string>();

                foreach (var fileInfo in dcmFilesInfo)
                {
                    var dcmFileStream = await _dicomService.GetDicomFileAsync(fileInfo.FilePath, file.Bucket);

                    // TODO
                    // Same comment as above

                    //if (dcmFileStream is null)
                    //{
                    //    continue;
                    //}

                    var dcmFile = await DicomFile.OpenAsync(dcmFileStream, FileReadOption.Default);

#pragma warning disable CS8601 // Possible null reference assignment.
                    // Study properties
                    if (string.IsNullOrWhiteSpace(clinicalReviewStudy.StudyUid))
                    {
                        clinicalReviewStudy.StudyUid = dcmFile.GetValueOrDefualt<string>(DicomTag.StudyInstanceUID);
                    }

                    if (clinicalReviewStudy.StudyDate == null || clinicalReviewStudy.StudyDate == DateTime.MinValue)
                    {
                        clinicalReviewStudy.StudyDate = dcmFile.GetValueOrDefualt<DateTime>(DicomTag.StudyDate);
                    }

                    if (string.IsNullOrWhiteSpace(clinicalReviewStudy.StudyDescription))
                    {
                        clinicalReviewStudy.StudyDescription = dcmFile.GetValueOrDefualt<string>(DicomTag.StudyDescription);
                    }

                    // Series properties
                    if (string.IsNullOrWhiteSpace(series.Id))
                    {
                        series.Id = dcmFile.GetValueOrDefualt<string>(DicomTag.SeriesInstanceUID);
                    }

                    if (string.IsNullOrWhiteSpace(series.Modality))
                    {
                        series.Modality = dcmFile.GetValueOrDefualt<string>(DicomTag.Modality);
                    }

                    filesDictionary.Add(
                        dcmFile.GetValueOrDefualt<int>(DicomTag.InstanceNumber),
                        fileInfo.FilePath
                    );
                }
#pragma warning restore CS8601 // Possible null reference assignment.

                series.Files = filesDictionary.Values.ToList();

                clinicalReviewStudy.Study.Add(series);
            }

            await _taskDetailsRepository.CreateTaskDetailsAsync(clinicalReviewStudy);
        }
    }
}
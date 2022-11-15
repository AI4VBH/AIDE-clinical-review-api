using System.Reflection;
using Aide.ClinicalReview.Service.IntegrationTests.POCO;
using TechTalk.SpecFlow.Infrastructure;

namespace Aide.ClinicalReview.Service.IntegrationTests.Support
{
    public class MinioDataSeeding
    {
        private MinioClientUtil MinioClient { get; set; }

        private ISpecFlowOutputHelper OutputHelper { get; set; }

        public MinioDataSeeding(MinioClientUtil minioClient, ISpecFlowOutputHelper outputHelper)
        {
            MinioClient = minioClient;
            OutputHelper = outputHelper;
        }

        public async Task SeedWorkflowInputArtifacts(string payloadId, string? folderName = null)
        {
            string localPath;

            if (string.IsNullOrEmpty(folderName))
            {
                OutputHelper.WriteLine($"folderName not specified. Seeding Minio with objects from **/DICOMs/full_patient_metadata/dcm");

                localPath = Path.Combine(GetDirectory(), "DICOMs", "full_patient_metadata", "dcm");
            }
            else
            {
                OutputHelper.WriteLine($"Seeding Minio with artifacts from **/DICOMs/{folderName}/dcm");

                localPath = Path.Combine(GetDirectory(), "DICOMs", folderName, "dcm");
            }

            OutputHelper.WriteLine($"Seeding objects to {TestExecutionConfig.MinioConfig.Bucket}/{payloadId}/dcm");
            await MinioClient.AddFileToStorage(localPath, $"{payloadId}/dcm");
            OutputHelper.WriteLine($"Objects seeded");
        }

        public async Task SeedTaskOutputArtifacts(string payloadId, string workflowInstanceId, string executionId, string? folderName = null)
        {
            string localPath;

            if (string.IsNullOrEmpty(folderName))
            {
                OutputHelper.WriteLine($"folderName not specified. Seeding Minio with objects from **/DICOMs/output_metadata/dcm");

                localPath = Path.Combine(GetDirectory(), "DICOMs", "output_metadata", "dcm");
            }
            else
            {
                OutputHelper.WriteLine($"Seeding Minio with objects from **/DICOMs/{folderName}/dcm");

                localPath = Path.Combine(GetDirectory(), "DICOMs", folderName, "dcm");
            }

            OutputHelper.WriteLine($"Seeding objects to {TestExecutionConfig.MinioConfig.Bucket}/{payloadId}/workflows/{workflowInstanceId}/{executionId}/");
            await MinioClient.AddFileToStorage(localPath, $"{payloadId}/workflows/{workflowInstanceId}/{executionId}/");
            OutputHelper.WriteLine($"Objects seeded");
        }

        private string GetDirectory()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

    }
}

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

        public async Task SeedArtifacts(string folder, string relativePath)
        {
            OutputHelper.WriteLine($"Seeding Minio with artifacts from **/DICOMs/study");
            var localPath = Path.Combine(GetDirectory(), "DICOMs", relativePath);
            await MinioClient.AddFileToStorage(localPath, $"{folder}/{relativePath}");
            OutputHelper.WriteLine($"Objects seeded");
        }

        private string GetDirectory()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

    }
}

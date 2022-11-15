using Aide.ClinicalReview.Service.IntegrationTests.POCO;
using Minio;
using Polly;
using Polly.Retry;
using System.Reactive.Linq;


namespace Aide.ClinicalReview.Service.IntegrationTests.Support
{
    public class MinioClientUtil
    {
        private AsyncRetryPolicy RetryPolicy { get; set; }
        private MinioClient Client { get; set; }

        public MinioClientUtil()
        {
            Client = new MinioClient()
                .WithEndpoint(TestExecutionConfig.MinioConfig.Endpoint)
                .WithCredentials(
                    TestExecutionConfig.MinioConfig.AccessKey,
                    TestExecutionConfig.MinioConfig.AccessToken
                ).Build();

            RetryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(retryCount: 10, sleepDurationProvider: _ => TimeSpan.FromMilliseconds(500));
        }

        public async Task CreateBucket(string bucketName)
        {
            await RetryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    if (await Client.BucketExistsAsync(bucketName))
                    {
                        try
                        {
                            var listOfKeys = new List<string>();
                            var listArgs = new ListObjectsArgs()
                                .WithBucket(bucketName)
                                .WithPrefix("")
                                .WithRecursive(true);

                            var objs = await Client.ListObjectsAsync(listArgs).ToList();
                            foreach (var obj in objs)
                            {
                                await Client.RemoveObjectAsync(bucketName, obj.Key);
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else
                    {
                        await Client.MakeBucketAsync(bucketName);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"[Bucket]  Exception: {e}");
                    if (e.Message != "MinIO API responded with message=Your previous request to create the named bucket succeeded and you already own it.")
                    {
                        throw;
                    }
                }
            });
        }

        public async Task AddFileToStorage(string localPath, string folderPath)
        {
            await RetryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    FileAttributes fileAttributes = File.GetAttributes(localPath);
                    if (fileAttributes.HasFlag(FileAttributes.Directory))
                    {
                        var files = Directory.GetFiles($"{localPath}", "*.*", SearchOption.AllDirectories);
                        foreach (var file in files)
                        {
                            var relativePath = $"{folderPath}{Path.GetRelativePath(localPath, file)}";
                            var fileName = Path.GetFileName(file);
                            var bs = File.ReadAllBytes(file);
                            using (var filestream = new MemoryStream(bs))
                            {
                                var fileInfo = new FileInfo(file);
                                var metaData = new Dictionary<string, string>
                                {
                                            { "Test-Metadata", "Test  Test" }
                                };
                                await Client.PutObjectAsync(
                                    TestExecutionConfig.MinioConfig.Bucket,
                                    relativePath,
                                    file,
                                    "application/octet-stream",
                                    metaData);
                            }
                        }
                    }
                    else
                    {
                        var bs = File.ReadAllBytes(localPath);
                        using (MemoryStream filestream = new MemoryStream(bs))
                        {
                            FileInfo fileInfo = new FileInfo(localPath);
                            var metaData = new Dictionary<string, string>
                        {
                                    { "Test-Metadata", "Test  Test" }
                        };
                            await Client.PutObjectAsync(
                                TestExecutionConfig.MinioConfig.Bucket,
                                folderPath,
                                localPath,
                                "application/octet-stream",
                                metaData);
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new Exception($"[Bucket]  Exception: {e}");
                }
            });
        }

        public async Task GetFile(string bucketName, string objectName, string fileName)
        {
            await Client.GetObjectAsync(bucketName, objectName, fileName);
        }

        public async Task<bool> CheckFileExists(string bucketName, string objectName)
        {
            var args = new StatObjectArgs()
                            .WithBucket(bucketName)
                            .WithObject(objectName);
            try
            {
                await Client.StatObjectAsync(args);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task DeleteBucket(string bucketName)
        {
            bool found = await Client.BucketExistsAsync(bucketName);
            if (found)
            {
                await RetryPolicy.ExecuteAsync(async () =>
                {
                    await Client.RemoveBucketAsync(bucketName);
                });
            }
        }

        public async Task RemoveObjects(string bucketName, string objectName)
        {
            if (await Client.BucketExistsAsync(bucketName))
            {
                await Client.RemoveObjectAsync(bucketName, objectName);
            }
        }
    }
}

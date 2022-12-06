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

using Aide.ClinicalReview.Service;
using Aide.ClinicalReview.Service.IntegrationTests.POCO;
using Aide.ClinicalReview.Service.IntegrationTests.Support;
using BoDi;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Retry;
using TechTalk.SpecFlow.Infrastructure;

namespace Monai.Deploy.WorkflowManager.TaskManager.IntegrationTests
{
    [Binding]
    public class Hooks
    {
        public Hooks(IObjectContainer objectContainer)
        {
            ObjectContainer = objectContainer;
        }

        private static RabbitPublisher? ClinicalReviewPublisher { get; set; }
        private static RabbitConsumer? TaskCallbackConsumer { get; set; }
        private static MinioClientUtil? MinioClient { get; set; }
        private static MongoClientUtil? MongoClient { get; set; }
        public static AsyncRetryPolicy? RetryPolicy { get; private set; }
        private IObjectContainer ObjectContainer { get; set; }
        private static WebApplicationFactory<Program>? WebApplicationFactory { get; set; }

        [BeforeTestRun(Order = 0)]
        public static void Init()
        {
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.Test.json")
                .Build();

            TestExecutionConfig.RabbitConfig.Host = config.GetValue<string>("AideClinicalReviewService:messaging:publisherSettings:endpoint");
            TestExecutionConfig.RabbitConfig.Port = 15672;
            TestExecutionConfig.RabbitConfig.User = config.GetValue<string>("AideClinicalReviewService:messaging:publisherSettings:username");
            TestExecutionConfig.RabbitConfig.Password = config.GetValue<string>("AideClinicalReviewService:messaging:publisherSettings:password");
            TestExecutionConfig.RabbitConfig.VirtualHost = config.GetValue<string>("AideClinicalReviewService:messaging:publisherSettings:virtualHost");
            TestExecutionConfig.RabbitConfig.Exchange = config.GetValue<string>("AideClinicalReviewService:messaging:publisherSettings:exchange");
            TestExecutionConfig.RabbitConfig.ClinicalReviewQueue = config.GetValue<string>("AideClinicalReviewService:messaging:topics:aideClinicalReviewRequest");
            TestExecutionConfig.RabbitConfig.TaskCallbackQueue = config.GetValue<string>("AideClinicalReviewService:messaging:topics:taskCallback");

            TestExecutionConfig.MongoConfig.ConnectionString = config.GetValue<string>("AideClinicalReviewDatabase:ConnectionString");
            TestExecutionConfig.MongoConfig.Database = config.GetValue<string>("AideClinicalReviewDatabase:DatabaseName");
            TestExecutionConfig.MongoConfig.AideClinicalReviewRecordCollection = config.GetValue<string>("AideClinicalReviewDatabase:AideClinicalReviewRecord");
            TestExecutionConfig.MongoConfig.AideClinicalReviewStudyCollection = config.GetValue<string>("AideClinicalReviewDatabase:AideClinicalReviewStudy");

            TestExecutionConfig.MinioConfig.Endpoint = config.GetValue<string>("AideClinicalReviewService:storage:settings:endpoint");
            TestExecutionConfig.MinioConfig.AccessKey = config.GetValue<string>("AideClinicalReviewService:storage:settings:accessKey");
            TestExecutionConfig.MinioConfig.AccessToken = config.GetValue<string>("AideClinicalReviewService:storage:settings:accessToken");
            TestExecutionConfig.MinioConfig.Bucket = config.GetValue<string>("AideClinicalReviewService:storage:settings:bucket");
            TestExecutionConfig.MinioConfig.Region = config.GetValue<string>("AideClinicalReviewService:storage:settings:region");

            TestExecutionConfig.ApiConfig.BaseUrl = "http://localhost:5000";
            TestExecutionConfig.ApiConfig.StudiesEndpoint = "/studies";
            TestExecutionConfig.ApiConfig.TasksEndpoint = "/clinical-review";
            TestExecutionConfig.ApiConfig.TaskDetailsEndpoint = "/task-details";
            TestExecutionConfig.ApiConfig.DicomEndpont = "/dicom";

            RabbitConnectionFactory.DeleteAllQueues();

            WebApplicationFactory = WebAppFactory.GetWebApplicationFactory();

            MongoClient = new MongoClientUtil();
            MinioClient = new MinioClientUtil();
            RetryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(retryCount: 20, sleepDurationProvider: _ => TimeSpan.FromMilliseconds(500));
            ClinicalReviewPublisher = new RabbitPublisher(TestExecutionConfig.RabbitConfig.Exchange, TestExecutionConfig.RabbitConfig.ClinicalReviewQueue);
            TaskCallbackConsumer = new RabbitConsumer(TestExecutionConfig.RabbitConfig.Exchange, TestExecutionConfig.RabbitConfig.TaskCallbackQueue);
        }

        [BeforeTestRun(Order = 2)]
        public static async Task CreateBucket()
        {
            await MinioClient.CreateBucket(TestExecutionConfig.MinioConfig.Bucket);
        }

        [BeforeTestRun(Order = 1)]
        [AfterTestRun(Order = 0)]
        [AfterScenario]
        public static void ClearTestData()
        {
            RabbitConnectionFactory.PurgeAllQueues();

            MongoClient?.DeleteAllClinicalReviewTasks();
            MongoClient?.DeleteAllClinicalReviewStudies();
        }

        [BeforeScenario]
        public void SetUp()
        {
            ObjectContainer.RegisterInstanceAs(ClinicalReviewPublisher);
            ObjectContainer.RegisterInstanceAs(TaskCallbackConsumer);
            ObjectContainer.RegisterInstanceAs(MongoClient);
            ObjectContainer.RegisterInstanceAs(MinioClient);
            ObjectContainer.RegisterInstanceAs(WebApplicationFactory?.CreateClient());
            ObjectContainer.RegisterInstanceAs(new DataHelper(ObjectContainer));
            ObjectContainer.RegisterInstanceAs(new MinioDataSeeding(MinioClient, ObjectContainer.Resolve<ISpecFlowOutputHelper>()));
        }

        [AfterTestRun(Order = 1)]
        public static void TearDown()
        {
            ClinicalReviewPublisher?.CloseConnection();
            WebApplicationFactory?.Dispose();
        }
    }
}
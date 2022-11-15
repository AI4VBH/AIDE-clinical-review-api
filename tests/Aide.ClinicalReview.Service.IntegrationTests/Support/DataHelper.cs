using Aide.ClinicalReview.Contracts.Messages;
using Aide.ClinicalReview.Contracts.Models;
using Aide.ClinicalReview.Service.IntegrationTests.POCO;
using BoDi;
using Monai.Deploy.Messaging.Messages;
using Polly;
using Polly.Retry;
using System.Reflection;
using System.Text.Json;
using TechTalk.SpecFlow.Infrastructure;

namespace Aide.ClinicalReview.Service.IntegrationTests.Support
{
    public class DataHelper
    {
        private MongoClientUtil MongoClient { get; set; }
        private RabbitConsumer TaskCallbackConsumer { get; set; }
        private RabbitPublisher ClinicalReviewPublisher { get; set; }
        private ISpecFlowOutputHelper OutputHelper { get; set; }
        private ApiHelper ApiHelper { get; }
        private RetryPolicy<string> RetryTaskCallback { get; set; }
        private string ExecutionId { get; set; }
        private ClinicalReviewRecord ClinicalReviewTask { get; set; }
        private ClinicalReviewStudy ClinicalReviewStudy { get; set; }
        public List<ClinicalReviewRecord> ClinicalReviewTasks { get; set; } = new List<ClinicalReviewRecord>();
        public List<ClinicalReviewStudy> ClinicalReviewStudies { get; set; } = new List<ClinicalReviewStudy>();
        public AideClinicalReviewRequestMessage ClinicalReviewEvent { get; set; }

        public DataHelper(IObjectContainer objectContainer)
        {
            MongoClient = objectContainer.Resolve<MongoClientUtil>() ?? throw new ArgumentNullException(nameof(MongoClientUtil));
            TaskCallbackConsumer = objectContainer.Resolve<RabbitConsumer>() ?? throw new ArgumentNullException(nameof(RabbitConsumer));
            ClinicalReviewPublisher = objectContainer.Resolve<RabbitPublisher>() ?? throw new ArgumentNullException(nameof(RabbitPublisher));
            ApiHelper = objectContainer.Resolve<ApiHelper>() ?? throw new ArgumentNullException(nameof(ApiHelper));
            OutputHelper = objectContainer.Resolve<ISpecFlowOutputHelper>();
            RetryTaskCallback = Policy<string>.Handle<Exception>().WaitAndRetry(retryCount: 10, sleepDurationProvider: _ => TimeSpan.FromMilliseconds(500));
        }

        public async Task SendClinicalReviewRequest(string action)
        {
            ApiHelper.SetUrl($"{TestExecutionConfig.ApiConfig.BaseUrl}/endpoint");
            ApiHelper.SetRequestVerb("POST");
            ApiHelper.Request.AddJsonBody("test");
            _ = await ApiHelper.GetResponseAsync();
        }

        public async Task<HttpResponseMessage> GetClinicalReviewTasks()
        {
            ApiHelper.SetUrl($"{TestExecutionConfig.ApiConfig.BaseUrl}/{TestExecutionConfig.ApiConfig.TasksEndpoint}");
            ApiHelper.SetRequestVerb("GET");
            return await ApiHelper.GetResponseAsync();
        }

        public async Task<HttpResponseMessage> GetClinicalReviewStudies()
        {
            ApiHelper.SetUrl($"{TestExecutionConfig.ApiConfig.BaseUrl}/endpoint");
            ApiHelper.SetRequestVerb("GET");
            return await ApiHelper.GetResponseAsync();
        }

        public void CreateClinicalReviewTask(string name)
        {
            OutputHelper.WriteLine($"Creating ClinicalReviewTask with name={name}");

            try
            {
                using var reader = new StreamReader(Path.Combine(GetBinDir(), "TestData", "ClinicalReviewTask", name));
                string json = reader.ReadToEnd();
                ClinicalReviewTask = JsonSerializer.Deserialize<ClinicalReviewRecord>(json);
                ClinicalReviewTasks.Add(ClinicalReviewTask);
            }
            catch (Exception)
            {
                throw new Exception($"Something went wrong deserializing {name}, please review!");
            }

            ExecutionId = ClinicalReviewTask.Id;
            MongoClient.CreateClinicalReviewTask(ClinicalReviewTask);

            OutputHelper.WriteLine($"ClinicalReviewTask with name={name} created!");
        }

        public void CreateClinicalReviewStudy(string name)
        {
            OutputHelper.WriteLine($"Creating a ClinicalReviewStudy with name={name}");

            try
            {
                using var reader = new StreamReader(Path.Combine(GetBinDir(), "TestData", "ClinicalReviewStudy", name));
                string json = reader.ReadToEnd();
                ClinicalReviewStudy = JsonSerializer.Deserialize<ClinicalReviewStudy>(json);
                ClinicalReviewStudies.Add(ClinicalReviewStudy);
            }
            catch (Exception)
            {
                throw new Exception($"Something went wrong deserializing {name}, please review!");
            }

            ExecutionId = ClinicalReviewStudy.ExecutionId;
            MongoClient.CreateClinicalReviewStudy(ClinicalReviewStudy);

            OutputHelper.WriteLine($"ClinicalReviewStudy with name={name} created!");
        }

        public List<ClinicalReviewRecord> GetClinicalReviewTasksFromEvent()
        {
            if (!string.IsNullOrEmpty(ClinicalReviewEvent.ExecutionId))
            {
                return MongoClient.GetClinicalReviewTaskByExecutionId(ClinicalReviewEvent.ExecutionId);
            }
            else
            {
                throw new Exception("Clinical Review ExecutionId id null or empty. Cannot retrive Clinical Review Task");
            }
        }

        public List<ClinicalReviewStudy> GetClinicalReviewStudiesFromEvent()
        {
            if (!string.IsNullOrEmpty(ClinicalReviewEvent.ExecutionId))
            {
                return MongoClient.GetClinicalReviewStudyByExecutionId(ClinicalReviewEvent.ExecutionId);
            }
            else
            {
                throw new Exception("Clinical Review ExecutionId id null or empty. Cannot retrive Clinical Review Study");
            }
        }

        public object GetTaskCallbackEvent()
        {
            OutputHelper.WriteLine($"Retreiving Task Callback Event for executionId");

            var res = RetryTaskCallback.Execute(() =>
            {
                var message = TaskCallbackConsumer.GetMessage<string>();

                if (message != null)
                { 
                    // check for rabbit message
                }

                throw new Exception($"TaskCallbackEvent not published for executionId");
            });

            return res;
        }

        public void PublishClinicalReviewRequestEvent(string name)
        {
            try
            {
                using var reader = new StreamReader(Path.Combine(GetBinDir(), "TestData", "ClinicalReviewEvent", name));
                string json = reader.ReadToEnd();
                ClinicalReviewEvent = JsonSerializer.Deserialize<AideClinicalReviewRequestMessage>(json);

                var message = new JsonMessage<string>(
                    json,
                    "16988a78-87b5-4168-a5c3-2cfc2bab8e54",
                    Guid.NewGuid().ToString(),
                    string.Empty);

                ClinicalReviewPublisher.PublishMessage(message.ToMessage());
            }
            catch (Exception)
            {
                throw new Exception($"Something went wrong deserializing {name}, please review!");
            }

            OutputHelper.WriteLine($"Successfully published ClinicalReviewRequestEvent with name={name}");
        }

        private string GetBinDir()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
    }
}

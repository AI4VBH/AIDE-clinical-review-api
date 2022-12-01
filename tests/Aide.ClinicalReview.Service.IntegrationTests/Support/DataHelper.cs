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

using Aide.ClinicalReview.Contracts.Messages;
using Aide.ClinicalReview.Contracts.Models;
using Aide.ClinicalReview.Service.IntegrationTests.POCO;
using BoDi;
using Monai.Deploy.Messaging.Messages;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using System.Reflection;
using System.Web;
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
        public ClinicalReviewStudy ClinicalReviewStudies { get; set; } = new ClinicalReviewStudy();
        public AideClinicalReviewRequestMessage ClinicalReviewEvent { get; private set; }

        public DataHelper(IObjectContainer objectContainer)
        {
            MongoClient = objectContainer.Resolve<MongoClientUtil>() ?? throw new ArgumentNullException(nameof(MongoClientUtil));
            TaskCallbackConsumer = objectContainer.Resolve<RabbitConsumer>() ?? throw new ArgumentNullException(nameof(RabbitConsumer));
            ClinicalReviewPublisher = objectContainer.Resolve<RabbitPublisher>() ?? throw new ArgumentNullException(nameof(RabbitPublisher));
            ApiHelper = objectContainer.Resolve<ApiHelper>() ?? throw new ArgumentNullException(nameof(ApiHelper));
            OutputHelper = objectContainer.Resolve<ISpecFlowOutputHelper>();
            RetryTaskCallback = Policy<string>.Handle<Exception>().WaitAndRetry(retryCount: 10, sleepDurationProvider: _ => TimeSpan.FromMilliseconds(500));
        }

        public async Task SendClinicalReviewRequest(bool action)
        {
            ApiHelper.SetUrl($"{TestExecutionConfig.ApiConfig.BaseUrl}/endpoint");
            ApiHelper.SetRequestVerb("POST");
            ApiHelper.Request.AddJsonBody("test");
            _ = await ApiHelper.GetResponseAsync();
        }

        public async Task<HttpResponseMessage> EditClinicalReviewRequest(string body, string executionId)
        {
            using var reader = new StreamReader(Path.Combine(GetBinDir(), "TestData", "ClinicalReviewTask", body));
            string json = reader.ReadToEnd();
            var ClinicalReviewAcceptReject = JsonConvert.DeserializeObject<AcknowledgeClinicalReview>(json);
           // json = JsonConvert.SerializeObject(ClinicalReviewAcceptReject);

            ApiHelper.SetUrl($"{TestExecutionConfig.ApiConfig.BaseUrl}{TestExecutionConfig.ApiConfig.TasksEndpoint}/{executionId}");
            ApiHelper.SetRequestVerb("PUT");
            HttpRequestMessageExtensions.AddJsonBody(ApiHelper.Request, ClinicalReviewAcceptReject);
            return await ApiHelper.GetResponseAsync();
        }

        public async Task<HttpResponseMessage> GetClinicalReviewRequest(string executionId)
        {
            ApiHelper.SetUrl($"{TestExecutionConfig.ApiConfig.BaseUrl}{TestExecutionConfig.ApiConfig.TasksEndpoint}/{executionId}");
            ApiHelper.SetRequestVerb("GET");
            return await ApiHelper.GetResponseAsync();
    } 
        public async Task<HttpResponseMessage> GetClinicalReviewTasks(Dictionary<string, string> parameters = null)
        {
            var builder = new UriBuilder($"{TestExecutionConfig.ApiConfig.BaseUrl}{TestExecutionConfig.ApiConfig.TasksEndpoint}");

            if (parameters != null)
            {
                var query = HttpUtility.ParseQueryString(builder.Query);
                foreach (var parameter in parameters)
                {
                    query[parameter.Key] = parameter.Value;
                }

                builder.Query = query.ToString();
            }

            ApiHelper.SetUrl(builder.ToString());
            ApiHelper.SetRequestVerb("GET");
            return await ApiHelper.GetResponseAsync();
        }

        public async Task<HttpResponseMessage> GetClinicalReviewStudies(string executionId)
        {
            ApiHelper.SetUrl($"{TestExecutionConfig.ApiConfig.BaseUrl}{TestExecutionConfig.ApiConfig.TaskDetailsEndpoint}/{executionId}");
            ApiHelper.SetRequestVerb("GET");
            return await ApiHelper.GetResponseAsync();
        }

        public async Task<HttpResponseMessage> GetClinicalReviewDicoms(string file)
        {
            ApiHelper.SetUrl($"{TestExecutionConfig.ApiConfig.BaseUrl}{TestExecutionConfig.ApiConfig.DicomEndpont}?key={file}");
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
                ClinicalReviewTask = JsonConvert.DeserializeObject<ClinicalReviewRecord>(json);
                ClinicalReviewTasks.Add(ClinicalReviewTask);
            }
            catch (Exception e)
            {
                throw new Exception($"Something went wrong deserializing {name}, please review!");
            }

            ExecutionId = ClinicalReviewTask.Id;
            MongoClient.CreateClinicalReviewTask(ClinicalReviewTask);

            OutputHelper.WriteLine($"ClinicalReviewTask with name={name} created!");
        }

        public List<ClinicalReviewRecord> GetClinicalReviewTask(string executionId)
        {
            return MongoClient.GetClinicalReviewTaskByExecutionId(executionId);
        }

        public void CreateClinicalReviewStudy(string name)
        {
            OutputHelper.WriteLine($"Creating a ClinicalReviewStudy with name={name}");

            try
            {
                using var reader = new StreamReader(Path.Combine(GetBinDir(), "TestData", "ClinicalReviewStudy", name));
                string json = reader.ReadToEnd();
                ClinicalReviewStudy = JsonConvert.DeserializeObject<ClinicalReviewStudy>(json);
                ClinicalReviewStudies = ClinicalReviewStudy;
            }
            catch (Exception)
            {
                throw new Exception($"Something went wrong deserializing {name}, please review!");
            }

            ExecutionId = ClinicalReviewStudy.ExecutionId;
            MongoClient.CreateClinicalReviewStudy(ClinicalReviewStudy);

            OutputHelper.WriteLine($"ClinicalReviewStudy with name={name} created!");
        }

        public ClinicalReviewStudy GetClinicalReviewStudy(string name)
        {
            try
            {
                using var reader = new StreamReader(Path.Combine(GetBinDir(), "TestData", "ClinicalReviewStudy", name));
                string json = reader.ReadToEnd();
                ClinicalReviewStudy = JsonConvert.DeserializeObject<ClinicalReviewStudy>(json);
                return ClinicalReviewStudy;
            }
            catch (Exception)
            {
                throw new Exception($"Something went wrong deserializing {name}, please review!");
            };
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
                using var reader = new StreamReader(Path.Combine(GetBinDir(), "TestData", "ClinicalReviewRequestEvent", name));
                string json = reader.ReadToEnd();
                var crEvent = JsonConvert.DeserializeObject<AideClinicalReviewRequestMessage>(json);

                if (crEvent is null)
                {
                    throw new ArgumentNullException($"Event not found for {name}");
                }

                ClinicalReviewEvent = crEvent;

                var message = new JsonMessage<AideClinicalReviewRequestMessage>(
                    crEvent,
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

        public ClinicalReviewRecord DeserializeClinicalReviewTask(string name)
        {
            try
            {
                using var reader = new StreamReader(Path.Combine(GetBinDir(), "TestData", "ClinicalReviewTask", name));
                string json = reader.ReadToEnd();
                var clinicalReviewTask = JsonConvert.DeserializeObject<ClinicalReviewRecord>(json);
                return clinicalReviewTask;
            }
            catch (Exception)
            {
                throw new Exception($"Something went wrong deserializing {name}, please review!");
            }
        }

        private string GetBinDir()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
    }
}
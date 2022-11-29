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

namespace Aide.ClinicalReview.Service.IntegrationTests.POCO
{
    static class TestExecutionConfig
    {
        public static class RabbitConfig
        {
            public static string Host { get; set; } = string.Empty;

            public static int Port { get; set; }

            public static string User { get; set; } = string.Empty;

            public static string Password { get; set; } = string.Empty;

            public static string Exchange { get; set; } = string.Empty;

            public static string VirtualHost { get; set; } = string.Empty;

            public static string ClinicalReviewQueue { get; set; } = string.Empty;

            public static string TaskCallbackQueue { get; set; } = string.Empty;
        }

        public static class MongoConfig
        {
            public static string ConnectionString { get; set; } = string.Empty;

            public static int Port { get; set; }

            public static string User { get; set; } = string.Empty;

            public static string Password { get; set; } = string.Empty;

            public static string Database { get; set; } = string.Empty;

            public static string AideClinicalReviewStudyCollection { get; set; } = string.Empty;

            public static string AideClinicalReviewRecordCollection { get; set; } = string.Empty;
        }

        public static class MinioConfig
        {
            public static string Endpoint { get; set; } = string.Empty;

            public static string AccessKey { get; set; } = string.Empty;

            public static string AccessToken { get; set; } = string.Empty;

            public static string Bucket { get; set; } = string.Empty;

            public static string Region { get; set; } = string.Empty;
        }

        public static class ApiConfig
        {
            public static string BaseUrl { get; set; } = string.Empty;
            public static string TasksEndpoint { get; set; } = string.Empty;
            public static string StudiesEndpoint { get; set; } = string.Empty;
            public static string TaskDetailsEndpoint { get; set; } = string.Empty;
            public static string DicomEndpont { get; internal set; }
        }
    }
}
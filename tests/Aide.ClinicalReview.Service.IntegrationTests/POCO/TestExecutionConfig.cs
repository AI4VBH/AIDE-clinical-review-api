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
        }
    }
}

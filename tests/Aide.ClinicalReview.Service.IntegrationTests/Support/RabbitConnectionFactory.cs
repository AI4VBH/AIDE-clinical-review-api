using Aide.ClinicalReview.Service.IntegrationTests.POCO;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Aide.ClinicalReview.Service.IntegrationTests.Support
{
    public static class RabbitConnectionFactory
    {
        private static IModel? Channel { get; set; }

        public static IModel GetRabbitConnection()
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = TestExecutionConfig.RabbitConfig.Host,
                UserName = TestExecutionConfig.RabbitConfig.User,
                Password = TestExecutionConfig.RabbitConfig.Password,
                VirtualHost = TestExecutionConfig.RabbitConfig.VirtualHost
            };

            Channel = connectionFactory.CreateConnection().CreateModel();

            return Channel;
        }

        public static void DeleteQueue(string queueName)
        {
            if (Channel is null)
            {
                GetRabbitConnection();
            }

            Channel?.QueueDelete(queueName);
        }

        public static void PurgeQueue(string queueName)
        {
            if (Channel is null)
            {
                GetRabbitConnection();
            }

            Channel?.QueuePurge(queueName);
        }

        public static void DeleteAllQueues()
        {
            try
            {
                DeleteQueue(TestExecutionConfig.RabbitConfig.ClinicalReviewQueue);
                DeleteQueue(TestExecutionConfig.RabbitConfig.TaskCallbackQueue);
                DeleteQueue($"{TestExecutionConfig.RabbitConfig.ClinicalReviewQueue}-dead-letter");
                DeleteQueue($"{TestExecutionConfig.RabbitConfig.TaskCallbackQueue}-dead-letter");
            } 
            catch (OperationInterruptedException)
            {

            }
        }

        public static void PurgeAllQueues()
        {
            try
            {
                PurgeQueue(TestExecutionConfig.RabbitConfig.ClinicalReviewQueue);
                PurgeQueue(TestExecutionConfig.RabbitConfig.TaskCallbackQueue);
                PurgeQueue($"{TestExecutionConfig.RabbitConfig.ClinicalReviewQueue}-dead-letter");
                PurgeQueue($"{TestExecutionConfig.RabbitConfig.TaskCallbackQueue}-dead-letter");
            }
            catch (OperationInterruptedException)
            {

            }
        }
    }
}

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

using Aide.ClinicalReview.Service.IntegrationTests.POCO;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Aide.ClinicalReview.Service.IntegrationTests.Support
{
    public static class RabbitConnectionFactory
    {
        public static IConnection? Connection { get; set; }

        public static void SetRabbitConnection()
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = TestExecutionConfig.RabbitConfig.Host,
                UserName = TestExecutionConfig.RabbitConfig.User,
                Password = TestExecutionConfig.RabbitConfig.Password,
                VirtualHost = TestExecutionConfig.RabbitConfig.VirtualHost
            };

            Connection = connectionFactory.CreateConnection();
        }

        public static void DeleteQueue(string queueName)
        {
            using (var channel = Connection?.CreateModel())
            {
                channel?.QueueDelete(queueName);
            }
        }

        public static void PurgeQueue(string queueName)
        {
            using (var channel = Connection?.CreateModel())
            {
                channel?.QueuePurge(queueName);
            }
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
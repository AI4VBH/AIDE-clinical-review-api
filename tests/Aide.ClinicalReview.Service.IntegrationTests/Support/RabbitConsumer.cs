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

using System.Text;
using System.Threading.Channels;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Aide.ClinicalReview.Service.IntegrationTests.Support
{
    public sealed class RabbitConsumer
    {
        public RabbitConsumer(string exchange, string routingKey)
        {
            Exchange = exchange;
            RoutingKey = routingKey;
            SetupQueue();
        }

        private QueueDeclareOk Queue { get; set; }

        private string Exchange { get; set; }

        private string RoutingKey { get; set; }

        public T GetMessage<T>()
        {
            using (var channel = RabbitConnectionFactory.Connection?.CreateModel())
            {
                var queue = channel.QueueDeclare(queue: RoutingKey, durable: true, exclusive: false, autoDelete: false);
                channel.QueueBind(queue.QueueName, Exchange, RoutingKey);
                channel.ExchangeDeclare(Exchange, ExchangeType.Topic, durable: true);

                var basicGetResult = channel.BasicGet(Queue.QueueName, true);

                if (basicGetResult != null)
                {
                    var byteArray = basicGetResult.Body.ToArray();

                    var str = Encoding.Default.GetString(byteArray);

                    return JsonConvert.DeserializeObject<T>(str);
                }
            }

            return default;
        }
        private void SetupQueue()
        {
            var Channel = RabbitConnectionFactory.GetRabbitConnection();
            Queue = Channel.QueueDeclare(queue: RoutingKey, durable: true, exclusive: false, autoDelete: false);
            Channel.QueueBind(Queue.QueueName, Exchange, RoutingKey);
            Channel.ExchangeDeclare(Exchange, ExchangeType.Topic, durable: true);
        }
    }
}
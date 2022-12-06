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

using Monai.Deploy.Messaging.Messages;
using RabbitMQ.Client;

namespace Aide.ClinicalReview.Service.IntegrationTests.Support
{
    public sealed class RabbitPublisher
    {
        public RabbitPublisher(string exchange, string routingKey)
        {
            Exchange = exchange;
            RoutingKey = routingKey;
        }

        private string Exchange { get; set; }

        private string RoutingKey { get; set; }

        private IModel Channel { get; set; }

        private IModel GetChannel()
        {
            Channel = RabbitConnectionFactory.GetRabbitConnection();
            Channel.ExchangeDeclare(Exchange, ExchangeType.Topic, durable: true);
            return Channel;
        }

        public void PublishMessage(Message message)
        {
            var propertiesDictionary = new Dictionary<string, object>
            {
                { "CreationDateTime", message.CreationDateTime.ToString("o") }
            };

            var properties = GetChannel().CreateBasicProperties();
            properties.Persistent = true;
            properties.ContentType = message.ContentType;
            properties.MessageId = message.MessageId;
            properties.AppId = message.ApplicationId;
            properties.CorrelationId = message.CorrelationId;
            properties.DeliveryMode = 2;
            properties.Headers = propertiesDictionary;
            properties.Type = message.MessageDescription;

            Channel.BasicPublish(exchange: Exchange,
                routingKey: RoutingKey,
                basicProperties: properties,
                body: message.Body);
        }

        public void CloseConnection()
        {
            Channel.Close();
        }
    }
}
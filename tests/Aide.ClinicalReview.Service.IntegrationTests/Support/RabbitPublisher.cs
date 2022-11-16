using Monai.Deploy.Messaging.Messages;
using RabbitMQ.Client;

namespace Aide.ClinicalReview.Service.IntegrationTests.Support
{
    public sealed class RabbitPublisher
    {
        public RabbitPublisher(IModel channel, string exchange, string routingKey)
        {
            Exchange = exchange;
            RoutingKey = routingKey;
            Channel = channel;
            Channel.ExchangeDeclare(Exchange, ExchangeType.Topic, durable: true);
        }

        private string Exchange { get; set; }

        private string RoutingKey { get; set; }

        private IModel Channel { get; set; }

        public void PublishMessage(Message message)
        {
            var propertiesDictionary = new Dictionary<string, object>
            {
                { "CreationDateTime", message.CreationDateTime.ToString("o") }
            };

            var properties = Channel.CreateBasicProperties();
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

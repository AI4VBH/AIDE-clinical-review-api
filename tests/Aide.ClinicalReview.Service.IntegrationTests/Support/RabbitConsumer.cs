using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Aide.ClinicalReview.Service.IntegrationTests.Support
{
    public class RabbitConsumer
    {
        public RabbitConsumer(IModel channel, string exchange, string routingKey)
        {
            Exchange = exchange;
            RoutingKey = routingKey;
            Channel = channel;
            Queue = Channel.QueueDeclare(queue: routingKey, durable: true, exclusive: false, autoDelete: false);
            Channel.QueueBind(Queue.QueueName, Exchange, RoutingKey);
            Channel.ExchangeDeclare(Exchange, ExchangeType.Topic, durable: true);
        }

        private QueueDeclareOk Queue { get; set; }

        private string Exchange { get; set; }

        private string RoutingKey { get; set; }

        private IModel Channel { get; set; }

        public T GetMessage<T>()
        {
            var basicGetResult = Channel.BasicGet(Queue.QueueName, true);

            if (basicGetResult != null)
            {
                var byteArray = basicGetResult.Body.ToArray();

                var str = Encoding.Default.GetString(byteArray);

                return JsonConvert.DeserializeObject<T>(str);
            }

            return default;
        }

        public void CloseConnection()
        {
            Channel.Close();
        }
    }
}

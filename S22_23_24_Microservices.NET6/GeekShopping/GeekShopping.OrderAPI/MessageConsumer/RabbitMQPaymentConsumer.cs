using GeekShopping.OrderAPI.Messages;
using GeekShopping.OrderAPI.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace GeekShopping.OrderAPI.MessageConsumer
{
    public class RabbitMQPaymentConsumer : BackgroundService
    {
        private readonly OrderRepository _repository;
        private IConnection _connection;
        private IModel _channel;
        private const string EXCHANGE_NAME = "DirectPaymentUpdateExchange";
        private const string PAYMENT_ORDER_UPDATE_QUEUE_NAME = "PaymentOrderUpdateQueueName";

        public RabbitMQPaymentConsumer(OrderRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(EXCHANGE_NAME, ExchangeType.Direct);
            _channel.QueueDeclare(PAYMENT_ORDER_UPDATE_QUEUE_NAME, false, false, false, null);

            _channel.QueueBind(PAYMENT_ORDER_UPDATE_QUEUE_NAME, EXCHANGE_NAME, "PaymentOrder");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (channel, evt) =>
            {
                var content = Encoding.UTF8.GetString(evt.Body.ToArray());

                UpdatePaymentResult payment = JsonSerializer.Deserialize<UpdatePaymentResult>(content);
                UpdatePaymentStatus(payment).GetAwaiter().GetResult();

                _channel.BasicAck(evt.DeliveryTag, false);
            };

            _channel.BasicConsume(PAYMENT_ORDER_UPDATE_QUEUE_NAME, false, consumer);

            return Task.CompletedTask;
        }

        private async Task UpdatePaymentStatus(UpdatePaymentResult payment)
        {
            try
            {
                await _repository.UpdateOrderPaymentStatus(payment.OrderId, payment.Status);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

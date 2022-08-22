using System.Text;
using ItemService.EventProcessor;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ItemService.RabbitMqClient
{
    public class RabbitMqSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly string _nomeDaFila;
        private readonly IConnection _connection;
        private IModel _channel;
        private IEventProcessor _processorEvent;

        public RabbitMqSubscriber(IConfiguration configuration, IEventProcessor processorEvent)
        {
            _configuration = configuration;
            _connection = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMqHost"],
                Port = Int32.Parse(_configuration["RabbitMqPort"])
            }.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            _nomeDaFila = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: _nomeDaFila, exchange: "trigger", routingKey: "");
            _processorEvent = processorEvent;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            EventingBasicConsumer? consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ModuleHandle, ea) => 
            {
                ReadOnlyMemory<byte> body = ea.Body;
                string? message = Encoding.UTF8.GetString(body.ToArray());
                _processorEvent.Process(message);
            };

            _channel.BasicConsume(queue: _nomeDaFila, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }
    }
}
using RabbitMQ.Client;
using System.Text;

namespace UserShortUrlService.AsyncDataServices
{
    public class RabbitMQSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IChannel _channel;
        privatre readonly string _queueName;

        public RabbitMQSubcriber(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory 
            {
                HostName = _configuration["RabbitMQHost"],
                Port = _configuration["RabbitMQPort"]
            };
            try{
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "users", type: ExchangeType.Fanout);
                
                _queueName = _channel.QueueDeclare().QueueName;
                _channel.QueueBind(queue: queueName, exchange: "users", routingKey: string.Empty);
            }
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Waiting for a newly registered user...");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"--> Received message: {message}");
            };
            _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);
        }
    }
}

using AutoMapper;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using UserShortUrlService.Data.Repository;
using UserShortUrlService.DTO;
using UserShortUrlService.Model;

namespace UserShortUrlService.AsyncDataServices
{
    public class RabbitMQSubscriber : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queueName;

        public RabbitMQSubscriber(IConfiguration configuration, IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
            _mapper = mapper;

            var factory = new ConnectionFactory 
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };
            try{
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "users", type: ExchangeType.Fanout);
                
                _queueName = _channel.QueueDeclare().QueueName;
                _channel.QueueBind(queue: _queueName, exchange: "users", routingKey: string.Empty);
            }
            catch(Exception ex){
                Console.WriteLine("--> Error while connecting to message queue (subscriber), error: " + ex.Message);
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

                AddUserUsingMessage(message);
            };
            _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);

            return Task.CompletedTask;
        }

        private void AddUserUsingMessage(string message)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IUserShortUrlCodeRepository>();

                    try{
                        Console.WriteLine("--> Adding user using message: " + message);

                        UserPublishedDTO userPublishedDTO = JsonSerializer.Deserialize<UserPublishedDTO>(message);

                        repo.AddUser(_mapper.Map<User>(userPublishedDTO));
                        repo.SaveChanges();
                    }
                    catch(Exception ex){
                        Console.WriteLine("--> Error while adding user from message, error: " + ex.Message);
                    }
            }
        }
    }
}

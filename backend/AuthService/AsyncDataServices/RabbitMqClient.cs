using System.Text;
using System.Text.Json;
using AuthService.DTO;
using RabbitMQ.Client;

namespace AuthService.AsyncDataServices
{
    public class RabbitMqClient : IRabbitMqClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory 
            { 
                Uri = new Uri($"amqp://guest:guest@{configuration["RabbitMQHost"]}:{configuration["RabbitMQPort"]}")
            };
            try {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare("users", ExchangeType.Fanout);
            }
            catch(Exception ex){
                Console.WriteLine($"--> Error while connecting to RabbitMQ. Host: {_configuration["RabbitMQHost"]} Port: {_configuration["RabbitMQPort"]}\nError: {ex.Message}");
            }
        }
        public void PublishNewUser(UserPublishDTO userPublishDTO)
        {
            string message = JsonSerializer.Serialize(userPublishDTO);
            var body = Encoding.UTF8.GetBytes(message);
            
            Console.WriteLine("Sending message: " + message);

            try{
                // Send message
                _channel.BasicPublish
                (
                    exchange: "users",
                    routingKey: string.Empty,
                    basicProperties: null,
                    body: body
                ); 
            }
            catch(Exception ex){
                Console.WriteLine("--> Error while sending message: " + ex.Message);
            }  
        }

        public void Dispose()
        {
            Console.WriteLine("messagebus disposed");
            if (_channel.IsOpen){
                _channel.Close();
                _connection.Close();
            }
        }
    }
}
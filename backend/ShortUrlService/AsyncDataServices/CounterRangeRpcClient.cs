using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ShortUrlService.Helper.Counter;

namespace ShortUrlService.AsyncDataServices
{
    public class CounterRangeRpcClient : ICounterRangeRpcClient,IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _replyQueueName;
        private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> _callbackMapper = new();

        public CounterRangeRpcClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory { Uri = new Uri($"amqp://guest:guest@{configuration["RabbitMQHost"]}:{configuration["RabbitMQPort"]}")};

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _replyQueueName = _channel.QueueDeclare().QueueName;
            Console.WriteLine("--> Connecting..., _replyQueueName: " + _replyQueueName);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) => {
                if (!_callbackMapper.TryRemove(ea.BasicProperties.CorrelationId, out var tcs)) return;
                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);
                Console.WriteLine("--> Received response: " + response);
                tcs.TrySetResult(response);
            };

            _channel.BasicConsume(consumer: consumer, queue: _replyQueueName, autoAck: true);
        }

        public async Task<CounterRange> GetNextCounterRange()
        {
            Console.WriteLine("--> Getting next counter range...");

            var nextCounterRangeResponse = await CallAsync(string.Empty);
            Console.WriteLine("nextCounterRangeResponse: " + nextCounterRangeResponse);

            return JsonSerializer.Deserialize<CounterRange>(nextCounterRangeResponse);
        }

        private Task<string> CallAsync(string message, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("--> Calling RPC...");

            IBasicProperties props = _channel.CreateBasicProperties();
            var correlationid = Guid.NewGuid().ToString();
            props.CorrelationId = correlationid;
            props.ReplyTo = _replyQueueName;
            var messageBytes = Encoding.UTF8.GetBytes(message);
            var tcs = new TaskCompletionSource<string>();
            _callbackMapper.TryAdd(correlationid, tcs);
            Console.WriteLine("--> e");
            
             Console.WriteLine("--> Publishing");
            
            _channel.BasicPublish
            (
                exchange: string.Empty,
                routingKey: "get_next_counter_range_queue",
                basicProperties: props,
                body: messageBytes
            );

            cancellationToken.Register(() => _callbackMapper.TryRemove(correlationid, out _));

            Console.WriteLine("--> Done calling, returning now");

            return tcs.Task;
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}
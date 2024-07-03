using System.Collections;
using System.IO.Pipes;
using System.Text;
using System.Text.Json;
using System.Threading;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


CancellationTokenSource Cts = new();

string envname = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

Console.WriteLine("envname: " + envname);

// configuration to let us use appsettings.json
var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory()) // start looking for these files here
    .AddJsonFile($"appsettings.{envname}.json", optional: true, reloadOnChange: true);


IConfiguration config = builder.Build();

Console.WriteLine("Host: " + config["RabbitMQHost"] + ", Port: " + config["RabbitMQPort"]);


// Connect to message queue, and initialize counter
int count = 1;
var factory = new ConnectionFactory { HostName = config["RabbitMQHost"], Port = int.Parse(config["RabbitMQPort"])};
var connection = factory.CreateConnection();
var channel = connection.CreateModel();
channel.QueueDeclare(queue: "get_next_counter_range_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
var consumer = new EventingBasicConsumer(channel);
channel.BasicConsume(queue: "get_next_counter_range_queue", autoAck: false, consumer: consumer);

consumer.Received += (model, ea) => {
    Console.WriteLine("--> Received RPC request");

    string response = string.Empty;

    var body = ea.Body.ToArray();
    var props = ea.BasicProperties;
    var replyProps = channel.CreateBasicProperties();
    replyProps.CorrelationId =  props.CorrelationId;

    try{
        // We don't care about the message, we just return the next available number
        response = JsonSerializer.Serialize(new 
        {
            Start = count,
            Max = count + (100000 - 1)
        });
        count += 100000;

        Console.WriteLine("--> Sending response: " + response);
    }
    catch(Exception ex){
        Console.WriteLine("--> Error while processing RPC request, error: " + ex.Message);
    }
    finally{
        var responseBytes = Encoding.UTF8.GetBytes(response);
        channel.BasicPublish(exchange: string.Empty, routingKey: props.ReplyTo, basicProperties: replyProps, body: responseBytes);
        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
    }
};

Console.WriteLine("--> Awaiting RPC requests");

await Task.Delay(Timeout.Infinite, Cts.Token).ConfigureAwait(false);
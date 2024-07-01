
namespace UserShortUrlService.AsyncDataServices
{
    public class MessageBusClient : BackgroundService
    {
        private readonly IConfiguration _configuration;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Waiting for a newly registered user...");
        }
    }
}
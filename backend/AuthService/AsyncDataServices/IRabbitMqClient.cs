using AuthService.DTO;

namespace AuthService.AsyncDataServices
{
    public interface IRabbitMqClient
    {
        void PublishNewUser(UserPublishDTO userPublishDTO);
    }
}
using UserShortUrlService.Model;

namespace UserShortUrlService.SyncDataServices.Grpc
{
    public interface IUserDataClient
    {
        IEnumerable<User> ReturnAllUsers();
    }
}
using UserShortUrlService.Model;

namespace UserShortUrlService.Data.Repository
{
    public interface IUserShortUrlCodeRepository
    {
        IEnumerable<UserShortUrlCode> GetAllUserShortUrlCodes();
        IEnumerable<User> GetAllUsers();
        void AddUser(User user);
        bool SaveChanges();
    }
}
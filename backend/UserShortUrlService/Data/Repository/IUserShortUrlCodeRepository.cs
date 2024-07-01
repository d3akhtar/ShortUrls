using UserShortUrlService.Model;

namespace UserShortUrlService.Data.Repository
{
    public interface IUserShortUrlCodeRepository
    {
        IEnumerable<UserShortUrl> GetAllUserShortUrlCodes();
        IEnumerable<UserShortUrl> GetUserShortUrlCodes(string userId);
        IEnumerable<User> GetAllUsers();
        void AddUser(User user);
        Task<IEnumerable<UserShortUrl>> AddUserShortUrls(string userId, List<string> codes);
        UserShortUrl DeleteUserShortUrl(string userId, string code);
        bool DoesUserWithIdExist(string userId);
        bool DoesUserShortUrlExist(string userId, string code);
        bool SaveChanges();
    }
}
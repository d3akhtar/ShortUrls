using UserShortUrlService.Model;

namespace UserShortUrlService.Data.Repository
{
    public interface IUserShortUrlCodeRepository
    {
        IEnumerable<UserShortUrl> GetAllUserShortUrlCodes(string searchQuery, int pageNumber, int pageSize);
        IEnumerable<UserShortUrl> GetUserShortUrlCodes(string userId, string searchQuery, int pageNumber, int pageSize);
        IEnumerable<User> GetAllUsers();
        void AddUser(User user);
        Task<IEnumerable<UserShortUrl>> AddUserShortUrls(string userId, List<string> codes);
        UserShortUrl DeleteUserShortUrl(string userId, string code);
        bool DoesUserWithIdExist(string userId);
        bool DoesUserShortUrlExist(string userId, string code);
        bool SaveChanges();
    }
}
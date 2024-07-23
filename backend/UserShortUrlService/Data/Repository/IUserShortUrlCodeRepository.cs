using UserShortUrlService.Model;

namespace UserShortUrlService.Data.Repository
{
    public interface IUserShortUrlCodeRepository
    {
        IEnumerable<UserShortUrl> GetAllUserShortUrlCodes(string searchQuery, int pageNumber, int pageSize);
        IEnumerable<UserShortUrl> GetUserShortUrlCodes(int userId, string searchQuery, int pageNumber, int pageSize);
        IEnumerable<User> GetAllUsers();
        void AddUser(User user);
        Task<IEnumerable<UserShortUrl>> AddUserShortUrls(int userId, List<string> codes);
        UserShortUrl DeleteUserShortUrl(int userId, string code);
        bool DoesUserWithIdExist(int userId);
        bool DoesUserShortUrlExist(int userId, string code);
        bool SaveChanges();
    }
}
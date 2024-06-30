using UserShortUrlService.Model;

namespace UserShortUrlService.Data.Repository
{
    public interface IUserShortUrlCodeRepository
    {
        IEnumerable<UserShortUrlCode> GetAllUserShortUrlCodes();
    }
}
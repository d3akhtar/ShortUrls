using UserShortUrlService.Model;

namespace UserShortUrlService.Data.Repository
{
    public class UserShortUrlCodeRepository : IUserShortUrlCodeRepository
    {
        private readonly AppDbContext _db;

        public UserShortUrlCodeRepository(AppDbContext db)
        {
            _db = db;
        }

        public IEnumerable<UserShortUrlCode> GetAllUserShortUrlCodes()
        {
            return _db.UserShortUrlCodes;
        }
    }
}
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

        public void AddUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException();
            }

            _db.Users.Add(user);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _db.Users;
        }

        public IEnumerable<UserShortUrlCode> GetAllUserShortUrlCodes()
        {
            return _db.UserShortUrlCodes;
        }

        public bool SaveChanges()
        {
            return _db.SaveChanges() >= 0;
        }
    }
}
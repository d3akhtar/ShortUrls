using AuthService.Model;

namespace AuthService.Data.Repository
{
    public class UserManager : IUserManager
    {
        private readonly AppDbContext _db;

        public UserManager(AppDbContext db)
        {
            _db = db;
        }
        public void AddUser(User user)
        {
            throw new NotImplementedException();
        }

        public void FindUserWithEmail(string email)
        {
            throw new NotImplementedException();
        }

        public void GetUserById(string id)
        {
            throw new NotImplementedException();
        }

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
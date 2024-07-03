using AuthService.Model;

namespace AuthService.Data.Repository
{
    using System.Collections.Generic;
    using BCrypt.Net;
    public class UserManager : IUserManager
    {
        private readonly AppDbContext _db;

        public UserManager(AppDbContext db)
        {
            _db = db;
        }
        public void AddUser(User user)
        {
            if (user == null)
            {
                throw new NullReferenceException();
            }

            user.Role = "user";
            _db.Add(user);
        }

        public User FindUserWithEmail(string email)
        {
            return _db.Users.FirstOrDefault(u => u.Email == email);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _db.Users;
        }

        public User GetUserById(string id)
        {
            return _db.Users.FirstOrDefault(u => u.UserId == id);
        }

        public bool IsPasswordValid(User user, string password)
        {
            return BCrypt.Verify(password, user.HashedPassword);
        }

        public bool SaveChanges()
        {
            return _db.SaveChanges() >= 0;
        }
    }
}
using AuthService.Model;

namespace AuthService.Data.Repository
{
    public interface IUserManager
    {
        void AddUser(User user);
        IEnumerable<User> GetAllUsers();
        User FindUserWithEmail(string email);
        User GetUserById(string id);
        bool IsPasswordValid(User user, string password);
        bool SaveChanges();
    }
}
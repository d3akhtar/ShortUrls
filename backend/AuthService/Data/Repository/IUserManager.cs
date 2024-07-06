using AuthService.Model;

namespace AuthService.Data.Repository
{
    public interface IUserManager
    {
        User AddUser(User user);
        IEnumerable<User> GetAllUsers();
        User FindUserWithEmail(string email);
        User GetUserById(string id);
        string GetTokenString(User user);
        bool IsPasswordValid(User user, string password);
        bool SaveChanges();
    }
}
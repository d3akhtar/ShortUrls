using AuthService.Model;

namespace AuthService.Data.Repository
{
    public interface IUserManager
    {
        User AddUser(User user);
        IEnumerable<User> GetAllUsers();
        IEnumerable<User> GetUnverifiedUsers();
        User FindUserWithEmail(string email);
        User GetUserById(int id);
        User FindUserWithVerificationToken(string token);
        User FindUserWithPasswordResetToken(string token);
        void VerifyUser(User user);
        void DeleteUsers(IEnumerable<User> users);
        string GetTokenString(User user);
        string SetPasswordResetToken(User user);
        void SetUserPassword(User user, string newPassword);
        bool IsPasswordValid(User user, string password);
        bool SaveChanges();
    }
}
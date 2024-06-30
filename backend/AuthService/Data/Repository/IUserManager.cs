using AuthService.Model;

namespace AuthService.Data.Repository
{
    public interface IUserManager
    {
        void AddUser(User user);
        void FindUserWithEmail(string email);
        void GetUserById(string id);
        bool SaveChanges();
    }
}
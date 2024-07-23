using AuthService.Model;

namespace AuthService.Data.Repository
{
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;
    using BCrypt.Net;
    using Microsoft.IdentityModel.Tokens;

    public class UserManager : IUserManager
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _configuration;

        public UserManager(AppDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }
        public User AddUser(User user)
        {
            if (user == null)
            {
                throw new NullReferenceException();
            }

            user.Role = "user";
            user.VerificationToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(64)) + 
                                     Convert.ToHexString(Encoding.UTF8.GetBytes(user.UserId.ToString()));
            user.CreatedAt = DateTime.Now;

            _db.Add(user);
            return user;
        }

        public void DeleteUsers(IEnumerable<User> users)
        {
            _db.Users.RemoveRange(users);
        }

        public User FindUserWithEmail(string email)
        {
            return _db.Users.FirstOrDefault(u => u.Email == email);
        }

        public User FindUserWithPasswordResetToken(string token)
        {
            return _db.Users.FirstOrDefault(u => u.PasswordResetToken == token);
        }

        public User FindUserWithVerificationToken(string token)
        {
            return _db.Users.FirstOrDefault(u => u.VerificationToken == token);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _db.Users;
        }

        public string GetTokenString(User user)
        {
            string secretKey = _configuration["Jwt:Secret"];
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.ASCII.GetBytes(secretKey);

            SecurityTokenDescriptor descriptor = new()
            {
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("userId", user.UserId.ToString()),
                    new Claim("email", user.Email),
                    new Claim("username", user.Username),
                    new Claim("role", user.Role),
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(descriptor);
            string tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        public IEnumerable<User> GetUnverifiedUsers()
        {
            return _db.Users.Where(u => u.VerifiedAt == null);
        }

        public User GetUserById(int id)
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

        public string SetPasswordResetToken(User user)
        {
            string passwordResetToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(64)) + 
                                        Convert.ToHexString(Encoding.UTF8.GetBytes(user.Email.ToString())); 
            user.PasswordResetToken = passwordResetToken;
            user.ResetTokenExpires = DateTime.Now.AddDays(1);

            _db.Update(user);

            return passwordResetToken;
        }

        public void SetUserPassword(User user, string newPassword)
        {
            user.HashedPassword = BCrypt.HashPassword(newPassword);
            _db.Users.Update(user);
        }

        public void VerifyUser(User user)
        {
            user.VerifiedAt = DateTime.Now;
        }
    }
}
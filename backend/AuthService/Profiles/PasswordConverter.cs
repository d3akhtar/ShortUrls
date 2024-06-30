namespace AuthService.Profiles
{
    using AutoMapper;
    using BCrypt.Net;
    public class PasswordConverter : IValueConverter<string, string>
    {
        public string Convert(string sourceMember, ResolutionContext context) 
            => BCrypt.HashPassword(sourceMember);
        
    }
}
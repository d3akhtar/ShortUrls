using AuthService.DTO;

namespace AuthService.ExternalAuthServices
{
    public interface IGoogleAuth
    {
        Task<ExternalUserProfileDTO> GetUserInfo(string accessToken);
    }
}
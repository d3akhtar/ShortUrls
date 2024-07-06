using System.Text;
using System.Text.Json;
using System.Web;
using AuthService.Data.Repository;
using AuthService.DTO;
using Microsoft.OpenApi.Models;

namespace AuthService.ExternalAuthServices
{
    public class GoogleAuth : IGoogleAuth
    {
        private readonly IUserManager _userManager;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string userInfoEndpoint = "https://www.googleapis.com/oauth2/v3/userinfo";

        public GoogleAuth(IUserManager userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<ExternalUserProfileDTO> GetUserInfo(string accessToken)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            HttpResponseMessage response = await httpClient.GetAsync(userInfoEndpoint);

            return await response.Content.ReadFromJsonAsync<ExternalUserProfileDTO>();
        }
    }
}
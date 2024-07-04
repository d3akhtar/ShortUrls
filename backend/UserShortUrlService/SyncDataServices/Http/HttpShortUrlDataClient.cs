using System.Text.Json;
using UserShortUrlService.DTO;

namespace UserShortUrlService.SyncDataServices.Http
{
    public class HttpShortUrlDataClient : IShortUrlDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpclient;

        public HttpShortUrlDataClient(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpclient = httpClient;
        }
        public async Task<ShortUrlHttpResponseDTO> RequestForShortUrl(string code)
        {
            Console.WriteLine("--> Using HTTP Client to check if ShortUrl with code " + code + " exists");
            try{
                var response = await _httpclient.GetAsync(_configuration["HttpShortUrlService"] + code + "?avoidRedirection=true");

                if(response.StatusCode != System.Net.HttpStatusCode.NotFound){
                    var responseBody = await response.Content.ReadFromJsonAsync<ShortUrlHttpResponseDTO>();
                    Console.WriteLine("responseBody: " + responseBody);
                    return responseBody;
                }
                else{
                    return null;
                }
            }
            catch(Exception ex){
                Console.WriteLine("--> Unexpected error while using HTTP Client to check if ShortUrl with code " + code + " exists, error: " + ex.Message);
                return null;
            }
        }
    }
}
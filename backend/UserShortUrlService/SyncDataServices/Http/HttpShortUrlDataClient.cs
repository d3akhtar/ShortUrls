using System.Text.Json;

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
        public async Task<bool> IsShortUrlMapped(string code)
        {
            Console.WriteLine("--> Using HTTP Client to check if ShortUrl with code " + code + " exists");
            try{
                var response = await _httpclient.GetAsync(_configuration["HttpShortUrlService"] + code);

                return response.StatusCode != System.Net.HttpStatusCode.NotFound;
            }
            catch(Exception ex){
                Console.WriteLine("--> Error while using HTTP Client to check if ShortUrl with code " + code + " exists, error: " + ex.Message);
                return false;
            }
        }
    }
}
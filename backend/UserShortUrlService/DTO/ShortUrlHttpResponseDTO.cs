using System.Text.Json.Serialization;
using UserShortUrlService.Model;

namespace UserShortUrlService.DTO
{
    public class ShortUrlHttpResponseDTO
    {
        [JsonInclude]
        public string Message { get; set; }
        [JsonInclude]
        public ShortUrl ShortUrl { get; set; }
    }
}
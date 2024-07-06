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
        [JsonInclude]
        public string PngQrCodeImage { get; set; }
        [JsonInclude]
        public string SvgQrCodeImage { get; set; }
        [JsonInclude]
        public string AsciiQrCodeImage { get; set; }
    }
}
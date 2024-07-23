namespace UserShortUrlService.DTO
{
    public class UserShortUrlReadDTO
    {
        public string ShortUrlCode { get; set; }
        public string DestinationUrl {get; set;}
        public string PngQrCodeImage { get; set; }
        public string SvgQrCodeImage { get; set; }
        public string AsciiQrCodeImage { get; set; }
        public int UserId { get; set; }
        public UserReadDTO User { get; set; }
    }
}
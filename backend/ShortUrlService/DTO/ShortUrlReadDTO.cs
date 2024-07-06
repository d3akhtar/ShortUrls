namespace ShortUrlService.DTO
{
    public class ShortUrlReadDTO
    {
        public string Code { get; set; }
        public string DestinationUrl { get; set; }
        public bool IsAlias { get; set; }
        public string PngQrCodeImage { get; set; }
        public string SvgQrCodeImage { get; set; }
        public string AsciiQrCodeImage { get; set; }
    }
}
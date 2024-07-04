namespace UserShortUrlService.DTO
{
    public class UserShortUrlReadDTO
    {
        public string ShortUrlCode { get; set; }
        public string DestinationUrl {get; set;}
        public string UserId { get; set; }
        public UserReadDTO User { get; set; }
    }
}
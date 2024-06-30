using System.ComponentModel.DataAnnotations;

namespace ShortUrlService.Model
{
    public class ShortUrl
    {
        [Key]
        [Required]
        public string Code { get; set; }
        [Required]
        public string DestinationUrl { get; set; }
        [Required]
        public bool IsAlias { get; set; }
    }
}
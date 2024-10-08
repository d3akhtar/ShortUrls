using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserShortUrlService.Model
{
    public class UserShortUrl
    {
        [Key]
        public int Id { get; set; }
        public string ShortUrlCode { get; set; }
        public string DestinationUrl {get; set;}
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}

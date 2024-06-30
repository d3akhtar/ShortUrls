using System.ComponentModel.DataAnnotations;

namespace UserShortUrlService.Model
{
    public class User
    {
        [Key]
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
    }
}

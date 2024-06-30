using System.ComponentModel.DataAnnotations;

namespace AuthService.Model
{
    public class User
    {
        [Key]
        [Required]
        public string UserId { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string HashedPassword { get; set; }
    }
}
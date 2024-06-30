using System.ComponentModel.DataAnnotations;

namespace AuthService.DTO
{
    public class RegisterRequestDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }
        public string Role {get; set;}
    }
}
using System.ComponentModel.DataAnnotations;

namespace AuthService.Model
{
    public class User
    {
        [Key]
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        public string? HashedPassword { get; set; }
        public string Role {get; set;}

        public DateTime CreatedAt {get; set;}

        /* When user registers, we create this token, which is a series of random characters.
         * Then, the user uses this token and calls an endpoint using the token. We check the
         * token, if it is valid and it belongs to the user, the user is verified.
        */
        public string? VerificationToken {get; set;}
        public DateTime? VerifiedAt {get; set;}
        public string? PasswordResetToken {get; set;} // Similar idea with verification token
        public DateTime? ResetTokenExpires {get; set;} // Determines when password reset token expires
    }
}
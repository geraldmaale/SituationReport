using System.ComponentModel.DataAnnotations;

namespace SR.Shared.DTOs.User
{
    public class UserTokenDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
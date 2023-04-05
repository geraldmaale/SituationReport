using System.ComponentModel.DataAnnotations;

namespace SR.Shared.DTOs.User
{
    public class ForgottenVerificationDto
    {
        [Required(ErrorMessage = "Verification code cannot be empty")]
        public string VerificationCode { get; set; }
    }
}
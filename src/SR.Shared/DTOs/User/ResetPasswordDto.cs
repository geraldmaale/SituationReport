using System.ComponentModel.DataAnnotations;

namespace SR.Shared.DTOs.User
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "UserId is required")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [RegularExpression(@"[^\s]+", ErrorMessage = "Spaces Not Permitted")]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [RegularExpression(@"[^\s]+", ErrorMessage = "Spaces Not Permitted")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        
        [Required(ErrorMessage = "A code must be supplied for password reset.")]
        public string Code { get; set; }
    }
}
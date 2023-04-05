using System.ComponentModel.DataAnnotations;

namespace SR.Shared.DTOs.User
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Enter your old password")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Enter your new password")]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [RegularExpression(@"[^\s]+", ErrorMessage = "Spaces Not Permitted")]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [RegularExpression(@"[^\s]+", ErrorMessage = "Spaces Not Permitted")]
        [Display(Name = "Confirm your new Password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
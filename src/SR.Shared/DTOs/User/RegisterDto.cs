using System.ComponentModel.DataAnnotations;

namespace SR.Shared.DTOs.User
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Contact Name is required")]
        public string ContactName { get; set; }

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email Address is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Account name is required")]
        public string OrganizationName { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 6)]
        [RegularExpression(@"[^\s]+", ErrorMessage = "Spaces Not Permitted")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [RegularExpression(@"[^\s]+", ErrorMessage = "Spaces Not Permitted")]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        
        [Required(ErrorMessage = "You must accept the Terms and Conditions")]
        public bool AgreeToTerms { get; set; }
    }
}


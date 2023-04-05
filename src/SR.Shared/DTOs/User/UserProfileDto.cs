using System.ComponentModel.DataAnnotations;

namespace SR.Shared.DTOs.User
{
    public class UserProfileDto
    {
        [Required(ErrorMessage = "Full Name is required")]
        public string ContactName { get; set; }

        [Display(Name = "Email Address")]
        public string Email { get; set; }
        
        [Display(Name = "Organization name is required")]
        public string OrganizationName { get; set; }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone Number is required")]
        public string PhoneNumber { get; set; }

        public string Gender { get; set; }
        [Required(ErrorMessage = "username is required")]
        public string UserName { get; set; }


    }
}
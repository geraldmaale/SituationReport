using System.ComponentModel.DataAnnotations;

namespace SR.Shared.DTOs.User
{
    public class UserEmailDto
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "New email")]
        public string NewEmail { get; set; }
    }
}
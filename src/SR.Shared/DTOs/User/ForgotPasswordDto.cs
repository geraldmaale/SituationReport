using System.ComponentModel.DataAnnotations;

namespace SR.Shared.DTOs.User;

public class ForgotPasswordDto
{
    [Required(ErrorMessage = "Email is required")]
    [DataType(DataType.EmailAddress)]
    [EmailAddress]
    public string Email { get; set; }
}

public class ResendEmailDto
{
    [Required(ErrorMessage = "Email is required")]
    [DataType(DataType.EmailAddress)]
    [EmailAddress]
    public string Email { get; set; }
}
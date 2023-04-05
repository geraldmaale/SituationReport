using System.ComponentModel.DataAnnotations;

namespace SR.Shared.DTOs.User;

public class LoginDto
{
    [Required(ErrorMessage = "Enter email address")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Enter a valid password")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public bool RememberMe { get; set; }
}

public class CloseUserAccountDto
{
    public string UserId { get; set; }
    
    [Required(ErrorMessage = "Enter your password to close the account")]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "Please provide a reason for closing the account")]
    public string Reason { get; set; }
    
    public bool Consent { get; set; }
}
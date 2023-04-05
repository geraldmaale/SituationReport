using Microsoft.AspNetCore.Identity;

namespace SR.Shared.Identity;

public class ApplicationUser : IdentityUser
{
    [PersonalData] public string FullName { get; set; }
    [PersonalData] public Guid OfficerId { get; set; }
}

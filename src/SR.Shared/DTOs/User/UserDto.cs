namespace SR.Shared.DTOs.User;

public class UserDto
{
	public string Id { get; set; }
	public string Email { get; set; }
	public string PhoneNumber { get; set; }
	public string FullName { get; set; }
	public List<string> Roles { get; set; } = new();
}
    
public class UserRoleManipulationDto
{
	public string UserId { get; set; }
	public string RoleName { get; set; }
}
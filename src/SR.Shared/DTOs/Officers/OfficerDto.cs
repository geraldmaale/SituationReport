using SR.Shared.Entities;

namespace SR.Shared.DTOs.Officers;
public record OfficerDto
{
	public Guid OfficerId { get; set; }
	public string FullName { get; set; }
	public string Gender { get; set; }
	public string Email { get; set; }
	public string PhoneNumber { get; set; }
	public string Rank { get; set; }
	public string Username { get; set; }
	public string UserId { get; set; }
}

public record OfficerFullDto
{
	public Guid OfficerId { get; set; }
	public string FullName { get; set; }
	public string FirstName { get; set; }
	public string MiddleName { get; set; }
	public string LastName { get; set; }
	public GenderEnum? Gender { get; set; }
	public string Email { get; set; }
	public string PhoneNumber { get; set; }
	public RankEnum? Rank { get; set; }
	public string Notes { get; set; }
	public string Username { get; set; }
	public string Password { get; set; }
	public string UserId { get; set; }
}


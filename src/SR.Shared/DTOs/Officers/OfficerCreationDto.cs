using System.ComponentModel.DataAnnotations;
using SR.Shared.Entities;

namespace SR.Shared.DTOs.Officers;

public class OfficerCreationDto
{
	public string FirstName { get; set; }
	public string MiddleName { get; set; }
	public string LastName { get; set; }
	public GenderEnum? Gender { get; set; }
	[EmailAddress]
	public string Email { get; set; }
	public string PhoneNumber { get; set; }
	public RankEnum? Rank { get; set; }
	public string Notes { get; set; }
	public string Username { get; set; }
	public string Password { get; set; }
	public string FullName
	{
		get
		{
			if (string.IsNullOrWhiteSpace(MiddleName))
			{
				return $"{FirstName} {LastName}";
			}
			else
			{
				return $"{FirstName} {MiddleName} {LastName}";
			}
		}
	}
}

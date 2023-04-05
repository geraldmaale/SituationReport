using SR.Shared.Identity;

namespace SR.Shared.Entities;

public record Officer
{
	public Guid OfficerId { get; init; }
	public string FirstName { get; init; }
	public string MiddleName { get; init; }
	public string LastName { get; init; }
	public GenderEnum Gender { get; init; }
	public string Email { get; init; }
	public string PhoneNumber { get; init; }
	public RankEnum Rank { get; init; }
	public string Username { get; set; }
	public string Notes { get; set; }
	public ApplicationUser User { get; set; }

	public string FullName { get; init; }

	public ICollection<CrewProcessing> CrewProcessings { get; set; }
	public ICollection<PermitProcessing> PermitProcessings { get; set; }
	public ICollection<PassportProcessing> PassportProcessings { get; set; }
	public ICollection<RevenueCollection> RevenueCollections { get; set; }
	public ICollection<OperationInspection> OperationInspections { get; set; }
}
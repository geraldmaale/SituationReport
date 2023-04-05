namespace SR.Shared.Entities;

public record OperationOffice
{
	public Guid OperationOfficeId { get; init; }
	public string OfficeName { get; init; }
	public string Location { get; init; }
	public string Role { get; init; }
	public ICollection<OperationInspection> OperationInspections { get; init; }
}
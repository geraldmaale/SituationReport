namespace SR.Shared.DTOs.Passports;
public class PassportUnitDto
{
	public Guid Id { get; init; }
	public DateTime EntryDate { get; init; }
	public Guid OfficerId { get; init; }
	public string Officer { get; init; }
	public PassportProcessingManipulationDto PermitProcessing { get; set; }
}

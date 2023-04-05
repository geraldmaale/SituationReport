namespace SR.Shared.DTOs.Passports;
public class PassportUnitManipulationDto
{
	public Guid Id { get; set; }
	public DateTime EntryDate { get; set; } = DateTime.UtcNow;
	public Guid OfficerId { get; set; }

	public PassportProcessingManipulationDto PassportProcessings { get; set; } = new ();
}

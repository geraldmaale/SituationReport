namespace SR.Shared.DTOs.Passports;
public class PassportProcessingManipulationDto
{
	public DateTime EntryDate { get; set; } = DateTime.UtcNow;
	public Guid OfficerId { get; set; }
	public int NumberOfNew { get; set; }
	public int NumberOfRenewal { get; set; }
	public int NumberOfMissing { get; set; }
	public int NumberOfChildren { get; set; }
	public int NumberOfAdults { get; set; }
	public int NumberDeclined { get; set; }
	public int TotalProcessed  => NumberOfNew + NumberOfRenewal + NumberOfMissing;
	public int TotalApplications => TotalProcessed + NumberDeclined;
}

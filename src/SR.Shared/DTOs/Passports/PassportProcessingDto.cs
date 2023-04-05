namespace SR.Shared.DTOs.Passports;
public class PassportProcessingDto
{
	public Guid Id { get; init; }
	public DateTime EntryDate { get; set; }
	public Guid OfficerId { get; set; }
	public string Officer { get; set; }
	public int NumberOfNew { get; set; }
	public int NumberOfRenewal { get; set; }
	public int NumberOfMissing { get; set; }
	public int NumberOfChildren { get; set; }
	public int NumberOfAdults { get; set; }
	public int NumberDeclined { get; set; }
	public int TotalProcessed { get; set; }
	public int TotalApplications { get; set; }
}

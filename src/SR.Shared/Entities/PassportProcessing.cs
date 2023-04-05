namespace SR.Shared.Entities;

public record PassportProcessing : EntityBase
{
	public int NumberOfNew { get; init; }
	public int NumberOfRenewal { get; init; }
	public int NumberOfMissing { get; init; }
	public int NumberOfChildren { get; init; }
	public int NumberOfAdults { get; init; }
	public int NumberDeclined { get; init; }
	public int TotalProcessed { get; init; }
	public int TotalApplications { get; init; }
}
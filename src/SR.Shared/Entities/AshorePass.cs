namespace SR.Shared.Entities;

public record AshorePass
{
	public Guid AshorePassId { get; init; }
	public int PassTypeId { get; init; }
	public int NumberOfPass { get; init; }
	public AshorePassType PassType { get; init; }
	public CrewProcessing CrewProcessing { get; init; }
};
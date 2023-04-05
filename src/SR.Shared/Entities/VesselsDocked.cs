
namespace SR.Shared.Entities;

public record VesselsDocked
{
	public Guid VesselDockedId { get; init; }
	public int VesselTypeId { get; init; }
	public Guid CrewProcessingId { get; init; }
	public string Name { get; init; }
	public int Count { get; init; }
	public VesselType VesselType { get; init; }
	public CrewProcessing CrewProcessing { get; init; }
};
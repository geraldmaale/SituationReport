namespace SR.Shared.Entities;

public record Embarkation
{
	public Guid EmbarkationId { get; init; }
	public Guid CrewProcessingId { get; init; }
	public int NationalityId { get; init; }
	public int Count { get; init; }
	public NationalityType Nationality { get; init; }
	public CrewProcessing CrewProcessing { get; init; }
};
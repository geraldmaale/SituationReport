namespace SR.Shared.Entities;

public record CrewProcessing : EntityBase
{
	public int Total { get; init; }
	public ICollection<Embarkation> Embarkations { get; init; }
	public ICollection<DisEmbarkation> DisEmbarkations { get; init; }
	public ICollection<VesselsDocked> VesselsDocked { get; init; }
	public ICollection<AshorePass> AshorePasses { get; init; }
}
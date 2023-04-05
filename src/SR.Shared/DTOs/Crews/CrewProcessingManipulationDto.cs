namespace SR.Shared.DTOs.Crews;

public class CrewProcessingManipulationDto
{
	public DateTime EntryDate { get; set; } = DateTime.UtcNow;
	public Guid OfficerId { get; set; }

	public int Total
	{
		get
		{
			var total = 0;
			var embarkation = Embarkations.Sum(x => x.Count);
			var disembarkation = DisEmbarkations.Sum(x => x.Count);
			var vesselDocked = VesselsDocked.Sum(x => x.Count);
			var ashorePass = AshorePasses.Sum(x => x.NumberOfPass);
			total = embarkation + disembarkation + vesselDocked + ashorePass;
			return total;
		}
	}
	public ICollection<EmbarkationDto> Embarkations { get; set; } = new List<EmbarkationDto>();
	public ICollection<DisEmbarkationDto> DisEmbarkations { get; set; } = new List<DisEmbarkationDto>();
	public ICollection<AshorePassDto> AshorePasses { get; set; } = new List<AshorePassDto>();
	public ICollection<VesselsDockedDto> VesselsDocked { get; set; } = new List<VesselsDockedDto>();
}
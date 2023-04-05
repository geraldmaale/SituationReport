namespace SR.Shared.DTOs.Crews;

public class CrewProcessingDto
{
	public Guid Id { get; set; }
	public DateTime EntryDate { get; set; }
	public Guid OfficerId { get; set; }
	public string Officer { get; set; }
	public int Total { get; set; }
	public ICollection<EmbarkationDto> Embarkations { get; set; }
	public ICollection<DisEmbarkationDto> DisEmbarkations { get; set; }
	public ICollection<VesselsDockedDto> VesselsDocked { get; set; }
}
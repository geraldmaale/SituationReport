
namespace SR.Shared.DTOs.Crews;

public class DisEmbarkationDto
{
	public Guid DisEmbarkationId { get; set; }
	public Guid CrewProcessingId { get; set; }
	public int NationalityId { get; set; }
	public string CrewNationality { get; set; }
	public int Count { get; set; } = 1;
}
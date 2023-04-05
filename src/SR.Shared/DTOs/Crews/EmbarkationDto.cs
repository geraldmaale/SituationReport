namespace SR.Shared.DTOs.Crews;

public record EmbarkationDto
{
	public Guid EmbarkationId { get; set; }
	public Guid CrewProcessingId { get; set; }
	public int NationalityId { get; set; }
	public string CrewNationality { get; set; }
	public int Count { get; set; } = 1;
};
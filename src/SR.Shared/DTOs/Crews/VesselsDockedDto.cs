
namespace SR.Shared.DTOs.Crews;

public class VesselsDockedDto
{
	public Guid VesselDockedId { get; set; }
	public int VesselTypeId { get; set; }
	public Guid CrewProcessingId { get; set; }
	public string VesselName { get; set; }
	public string VesselTypeName { get; set; }
	public int Count { get; set; } = 1;
};
using System.ComponentModel.DataAnnotations;

namespace SR.Shared.DTOs.Permits;
public class PermitProcessingManipulationDto
{
	public Guid Id { get; set; }
	public DateTime EntryDate { get; set; } = DateTime.UtcNow;
	public Guid OfficerId { get; set; }
	public string Penalties { get; set; }
	public string Remarks { get; set; }
	public int Total => PermitProcessingDetails.Count;
	
	[ValidateComplexType]
	public ICollection<PermitProcessingDetailManipulationDto> PermitProcessingDetails { get; set; } = new List<PermitProcessingDetailManipulationDto>();
}

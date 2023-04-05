using System.ComponentModel.DataAnnotations;
using SR.Shared.Entities;

namespace SR.Shared.DTOs.Permits;
public class PermitProcessingDetailManipulationDto
{
	public Guid PermitProcessingDetailId { get; set; }
	[Required]
	public string Name { get; set; }
	[Required]
	public GenderEnum Gender { get; set; }
	[Required]
	public int? NationalityId { get; set; }
	[Required]
	public int? PermitTypeId { get; set; }
	public string Duration { get; set; }
	public string PermitNumber { get; set; }
	public Guid PermitProcessingId { get; set; }
}

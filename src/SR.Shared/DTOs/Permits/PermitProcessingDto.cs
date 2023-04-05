namespace SR.Shared.DTOs.Permits;
public class PermitProcessingDto
{
	public Guid Id { get; set; }
	public DateTime EntryDate { get; set; }
	public Guid OfficerId { get; set; }
	public string OfficerName { get; set; }
	public string Penalties { get; set; }
	public string Remarks { get; set; }
	public int Total { get; set; }
	public ICollection<PermitProcessingDetailDto> PermitProcessingDetails { get; set; } = new List<PermitProcessingDetailDto>();
}

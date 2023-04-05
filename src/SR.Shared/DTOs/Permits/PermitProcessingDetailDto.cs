namespace SR.Shared.DTOs.Permits;
public class PermitProcessingDetailDto
{
	public Guid PermitProcessingDetailId { get; set; }
	public string Name { get; set; }
	public string Gender { get; set; }
	public int NationalityId { get; set; }
	public int PermitTypeId { get; set; }
	public string Duration { get; set; }
	public string PermitNumber { get; set; }
	public Guid PermitProcessingId { get; set; }
	public string PermitType { get; set; }
	public string Nationality { get; set; }
}

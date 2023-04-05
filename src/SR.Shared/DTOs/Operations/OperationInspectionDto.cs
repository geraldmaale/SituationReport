namespace SR.Shared.DTOs.Operations;

public record OperationInspectionDto
{
	public Guid Id { get; set; }
	public DateTime EntryDate { get; set; }
	public Guid OfficerId { get; set; }
	public string OfficerName { get; set; }
	public int Total { get; set; }
	public Guid OperationOfficeId { get; set; }
	public OperationOfficeDto OperationOfficeDto { get; set; }
	public ICollection<OperationInspectionDetailDto> OperationInspectionDetails { get; set; }
}
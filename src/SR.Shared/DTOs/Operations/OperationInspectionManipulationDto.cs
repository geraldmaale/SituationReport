namespace SR.Shared.DTOs.Operations;

public record OperationInspectionManipulationDto
{
	public DateTime EntryDate { get; set; } = DateTime.UtcNow;
	public Guid OfficerId { get; set; }
	public int Total => OperationInspectionDetails.Sum(x => x.Number);
	public Guid OperationOfficeId { get; set; }
	public ICollection<OperationInspectionDetailDto> OperationInspectionDetails { get; set; } =
		new List<OperationInspectionDetailDto>();
}
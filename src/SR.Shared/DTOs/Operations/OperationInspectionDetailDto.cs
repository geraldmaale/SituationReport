namespace SR.Shared.DTOs.Operations;

public record OperationInspectionDetailDto
{
	public Guid OperationInspectionDetailId { get; set; }
	public int Number { get; set; }
	public int? OperationTypeId { get; set; }
	public Guid OperationInspectionId { get; set; }
	public string OperationTypeName { get; set; }
}
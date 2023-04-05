namespace SR.Shared.Entities;

public record OperationInspectionDetail
{
	public Guid OperationInspectionDetailId { get; set; }
	public int Number { get; init; }
	public int OperationTypeId { get; init; }
	public Guid OperationInspectionId { get; init; }
	public OperationType OperationType { get; init; }
	public OperationInspection OperationInspection { get; init; }

}
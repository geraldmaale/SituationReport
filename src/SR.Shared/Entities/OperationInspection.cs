namespace SR.Shared.Entities;

public record OperationInspection : EntityBase
{
	public int Total { get; init; }
	public Guid OperationOfficeId { get; init; }
	public OperationOffice OperationOffice { get; init; }
	public ICollection<OperationInspectionDetail> OperationInspectionDetails { get; init; }
}
namespace SR.Shared.Entities;

public record PermitProcessing : EntityBase
{
	public string Penalties { get; init; }
	public string Remarks { get; init; }
	public int Total { get; set; }
	public ICollection<PermitProcessingDetail> PermitProcessingDetails { get; init; }
}
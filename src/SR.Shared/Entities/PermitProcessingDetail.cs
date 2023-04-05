namespace SR.Shared.Entities;

public record PermitProcessingDetail
{
	public Guid PermitProcessingDetailId { get; set; }
	public string Name { get; init; }
	public GenderEnum Gender { get; init; }
	public int NationalityId { get; init; }
	public int PermitTypeId { get; init; }
	public string Duration { get; init; }
	public string PermitNumber { get; init; }
	public Guid PermitProcessingId { get; set; }
	public PermitType PermitType { get; init; }
	public NationalityType Nationality { get; init; }
	public PermitProcessing PermitProcessing { get; set; }
}
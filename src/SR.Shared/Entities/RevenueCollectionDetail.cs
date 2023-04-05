namespace SR.Shared.Entities;

public record RevenueCollectionDetail
{
	public Guid RevenueCollectionDetailId { get; set; }
	public int RevenueTypeId { get; init; }
	public decimal Amount { get; init; }
	public RevenueType RevenueType { get; init; }
	public Guid RevenueCollectionId { get; set; }
	public RevenueCollection RevenueCollection { get; set; }
}
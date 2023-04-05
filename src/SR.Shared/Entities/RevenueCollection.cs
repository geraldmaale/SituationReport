namespace SR.Shared.Entities;

public record RevenueCollection: EntityBase
{
	public decimal TotalAmount { get; init; }
	public ICollection<RevenueCollectionDetail> RevenueCollectionDetails { get; init; }
}
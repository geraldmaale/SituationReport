
namespace SR.Shared.DTOs.Revenues;

public record RevenueCollectionDetailDto
{
	public Guid RevenueCollectionDetailId { get; set; }
	public int? RevenueTypeId { get; set; }
	public decimal Amount { get; set; }
	public Guid RevenueCollectionId { get; set; }
	public string RevenueTypeName { get; set; }
}
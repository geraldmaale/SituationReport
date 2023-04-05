namespace SR.Shared.DTOs.Revenues;

public class RevenueCollectionDto
{
	public Guid Id { get; set; }
	public DateTime EntryDate { get; set; }
	public Guid OfficerId { get; set; }
	public string Officer { get; set; }
	public decimal TotalAmount { get; set; }
	public ICollection<RevenueCollectionDetailDto> RevenueCollectionDetails { get; set; }
}
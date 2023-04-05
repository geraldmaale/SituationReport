namespace SR.Shared.DTOs.Revenues;

public class RevenueCollectionManipulationDto
{
	public Guid Id { get; set; }
	public DateTime EntryDate { get; set; } = DateTime.UtcNow;
	public Guid OfficerId { get; set; }
	public string OfficerName { get; set; }
	public decimal TotalAmount => RevenueCollectionDetails.Sum(x => x.Amount);
	public ICollection<RevenueCollectionDetailDto> RevenueCollectionDetails { get; set; } = new List<RevenueCollectionDetailDto>();
}
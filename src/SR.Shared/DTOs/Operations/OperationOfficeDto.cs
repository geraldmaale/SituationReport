namespace SR.Shared.DTOs.Operations;

public record OperationOfficeDto
{
	public Guid OperationOfficeId { get; set; }
	public string OfficeName { get; set; }
	public string Location { get; set; }
	public string Role { get; set; }
	public ICollection<OperationInspectionDto> OperationInspections { get; set; }
}
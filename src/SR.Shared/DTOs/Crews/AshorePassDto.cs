namespace SR.Shared.DTOs.Crews;

public class AshorePassDto
{
	public Guid AshorePassId { get; set; }
	public int PassTypeId { get; set; }
	public int NumberOfPass { get; set; } = 1;
	public string AshorePassType { get; init; }
}
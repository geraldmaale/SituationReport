namespace SR.Shared.DTOs.Crews;

public class CrewProcessingExportDto
{
	public List<EmbarkationExportDto> Embarkations { get; set; }
	public List<DisEmbarkationExportDto> Disembarkations { get; set; }
	public List<VesselDockedExportDto> VesselsDocked { get; set; }
	public List<AshorePassExportDto> AshorePasses { get; set; }
}

public class EmbarkationExportDto
{
	public string Nationality { get; set; }
	public int Number { get; set; }
}

public class DisEmbarkationExportDto
{
	public string Nationality { get; set; }
	public int Number { get; set; }
}

public class AshorePassExportDto
{
	public string AshorePass { get; set; }
	public int Number { get; set; }
}

public class VesselDockedExportDto
{
	public string Vessel { get; set; }
	public int Number { get; set; }
}
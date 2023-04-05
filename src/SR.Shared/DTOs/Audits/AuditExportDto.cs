namespace SR.Shared.DTOs.Audits;

public class AuditExportDto
{
	public string Timestamp { get; set; }
	public string Username { get; set; }
	public string FullName { get; set; }
	public string Action { get; set; }
	public string Entity { get; set; }
	public string PrimaryKey { get; set; }
	public string AffectedColumns { get; set; }
	// public string IpAddress { get; set; }
	public string OldValues { get; set; }
	public string NewValues { get; set; }
}
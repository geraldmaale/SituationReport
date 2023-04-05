using System.Net;

namespace SR.Shared.Entities;

public class Audit
{
	public Guid Id { get; set; }
	public string Username { get; set; }
	public string FullName { get; set; }
	public string Action { get; set; }
	public string TableName { get; set; }
	public DateTime Timestamp { get; set; }
	public string OldValues { get; set; }
	public string NewValues { get; set; }
	public string AffectedColumns { get; set; }
	public string PrimaryKey { get; set; }
	public IPAddress IpAddress { get; set; }
}
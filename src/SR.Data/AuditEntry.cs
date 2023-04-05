using System.Net;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using SR.Shared.Entities;

namespace SR.Data;

#nullable enable
public class AuditEntry
{
	public AuditEntry(EntityEntry entry)
	{
		Entry = entry;
	}

	public EntityEntry Entry { get; }
	public string? Username { get; set; }
	public string? FullName { get; set; }
	public string? TableName { get; set; }
	public IPAddress? IpAddress { get; set; }
	public Dictionary<string, object?> KeyValues { get; } = new();
	public Dictionary<string, object?> OldValues { get; } = new();
	public Dictionary<string, object?> NewValues { get; } = new();
	public AuditType AuditType { get; set; }
	public List<string> ChangedColumns { get; } = new ();
	public Audit ToAudit()
	{
		var audit = new Audit();
		audit.Username = Username;
		audit.FullName = FullName;
		audit.Action = AuditType.ToString();
		audit.IpAddress = IpAddress;
		audit.TableName = TableName;
		audit.Timestamp = DateTime.UtcNow;
		audit.PrimaryKey = JsonConvert.SerializeObject(KeyValues);
		audit.OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues);
		audit.NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues);
		audit.AffectedColumns = ChangedColumns.Count == 0 ? null : JsonConvert.SerializeObject(ChangedColumns);
		return audit;
	}
}

public enum AuditType
{
	None = 0,
	Create = 1,
	Update = 2,
	Delete = 3
}
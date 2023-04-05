using GreatIdeas.Extensions.Paging;

namespace SR.Shared.Params;

public class AuditPagingParameters: PagingParams
{
	public string Username { get; set; }
	// public string FullName { get; set; }
	public string Action { get; set; }
	public string Entity { get; set; }
	public DateTime? Timestamp { get; set; }
	public bool LoadData { get; set; }
	public string FileName { get; set; }
}
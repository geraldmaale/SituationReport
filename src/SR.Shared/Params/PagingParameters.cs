using GreatIdeas.Extensions.Paging;

namespace SR.Shared.Params;
public class PagingParameters : PagingParams
{
	public string Title { get; set; }
	public string ExportType { get; set; }
	public string FileName { get; set; }
}

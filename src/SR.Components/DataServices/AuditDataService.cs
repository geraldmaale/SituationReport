using GreatIdeas.Blazor.Extensions;
using GreatIdeas.Extensions;
using GreatIdeas.Extensions.Paging;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using Radzen;
using Serilog;
using SR.Data.Repositories.Contracts;
using SR.Shared;
using SR.Shared.DTOs.Audits;
using SR.Shared.Params;

namespace SR.Components.DataServices;

public class AuditDataService : IAuditDataService
{
	private readonly IAuditRepository _auditRepository;
	private readonly IJSRuntime _jsRuntime;
	private IMapper _mapper = new Mapper();

	public AuditDataService(IAuditRepository auditRepository, IJSRuntime jsRuntime)
	{
		_auditRepository = auditRepository;
		_jsRuntime = jsRuntime;
	}
	
	public async ValueTask<ApiResults<AuditDto>> GetAllAsync(CancellationToken cancellationToken)
	{
		try
		{
			var results = await _auditRepository.GetAllProjectToAsync(cancellationToken);
			return new ApiResults<AuditDto>()
			{
				Results = results.OrderByDescending(x=>x.Timestamp), 
				IsSuccessful = true, 
				Message = StatusLabels.GetSuccess("Audit logs")
			};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to get audit logs");
			return new ApiResults<AuditDto>() {Message = "Failed to get audit logs"};
		}
	}

	public async Task<PagingResponse<AuditDto>> GetPagedAsync(
		AuditPagingParameters pagingParameters, CancellationToken token)
	{
		try
		{
			var results = await _auditRepository.GetPagedAuditsAsync(pagingParameters, token);

			var pagingResponse = new PagingResponse<AuditDto>
			{
				IsSuccessful = true,
				Message = "Successfully retrieved paged audit logs.",
				Items =results,
				Metadata = results.Metadata
			};

			return pagingResponse;
		}
		catch (Exception e)
		{
			Log.Error(e, "Error retrieving paged audit logs");
			return new PagingResponse<AuditDto>
			{
				IsSuccessful = false,
				Message = "Error retrieving paged audit logs."
			};
		}
	}
	
	public async Task<PagingResponse<AuditDto>> GetPagedAsync(
		LoadDataArgs args, CancellationToken token)
	{
		try
		{
			var query = _auditRepository.GetAll();
			if (!string.IsNullOrWhiteSpace(args.Filter))
			{
				query = query.Where(x => x.FullName.Contains(args.Filter));
			}

			var pagingResponse = new PagingResponse<AuditDto>();
			
			if (!string.IsNullOrEmpty(args.OrderBy))
			{
				query = query.OrderBy(x=>x.FullName);
			}

			pagingResponse.Metadata = new Metadata() {TotalCount = query.Count()};
			
			var results = await query.Skip(args.Skip.Value).Take(args.Top.Value).ToListAsync(token);
			pagingResponse.Items = _mapper.Map<List<AuditDto>>(results);
			pagingResponse.IsSuccessful = true;
			pagingResponse.Message = "Successfully retrieved paged audit logs.";
			return pagingResponse;
		}
		catch (Exception e)
		{
			Log.Error(e, "Error retrieving paged audit logs");
			return new PagingResponse<AuditDto>
			{
				IsSuccessful = false,
				Message = "Error retrieving paged audit logs."
			};
		}
	}

	public async ValueTask<ApiResult<AuditDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
	{
		try
		{
			var results = await _auditRepository.GetWithProjectToAsync(
				x=>x.Id == id, cancellationToken: cancellationToken);
			// Log.Information("Successfully retrieved audit log with id {log}", JsonConvert.SerializeObject(results));
			return new ApiResult<AuditDto>()
			{
				Result = results, IsSuccessful = true, Message = StatusLabels.GetSuccess("Audit log")
			};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to get audit log");
			return new ApiResult<AuditDto>() {Message = "Failed to get audit log"};
		}
	}
	
	public async Task<ApiResult> ExportAsync(AuditPagingParameters pagingParams, CancellationToken token)
	{
		try
		{
			var results = await _auditRepository.Export(pagingParams, token);
			var fileBytes = results.ToArray();

			var fileName = $"{pagingParams.FileName}.xlsx";
			await _jsRuntime.SaveAs(fileName, fileBytes);

			return new ApiResult { IsSuccessful = true, Message = "Audit logs exported successfully" };
		}
		catch (Exception e)
		{
			Log.Error(e, "Failed to export audit logs");
			return new ApiResult { Message = "Failed to export audit logs" };
		}
	}
	
}
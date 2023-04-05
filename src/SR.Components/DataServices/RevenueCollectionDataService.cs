using GreatIdeas.Blazor.Extensions;
using GreatIdeas.Extensions;
using MapsterMapper;
using Microsoft.JSInterop;
using Serilog;
using SR.Data.Repositories.Contracts;
using SR.Shared;
using SR.Shared.DTOs.Revenues;
using SR.Shared.Entities;
using SR.Shared.Params;

namespace SR.Components.DataServices;

public class RevenueCollectionDataService : IRevenueCollectionDataService
{
	private readonly IRevenueRepository _revenueCollectionRepository;
	private readonly IMapper _mapper = new Mapper();
	private readonly IJSRuntime _jsRuntime;

	public RevenueCollectionDataService(IRevenueRepository revenueCollectionRepository, IJSRuntime jsRuntime)
	{
		_revenueCollectionRepository = revenueCollectionRepository;
		_jsRuntime = jsRuntime;
	}

	public async ValueTask<ApiResults<RevenueCollectionDto>> GetAllAsync(CancellationToken cancellationToken)
	{
		try
		{
			var results = await _revenueCollectionRepository.GetAllProjectToAsync(cancellationToken);
			return new ApiResults<RevenueCollectionDto>()
			{
				Results = results.OrderByDescending(x => x.EntryDate),
				IsSuccessful = true,
				Message = StatusLabels.GetSuccess("Revenue Collections")
			};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to get Revenue Collection");
			return new ApiResults<RevenueCollectionDto>() { Message = "Failed to get Revenue Collection" };
		}
	}

	public async ValueTask<ApiResult<RevenueCollection>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
	{
		try
		{
			var results = await _revenueCollectionRepository.GetById(id, cancellationToken);
			return new ApiResult<RevenueCollection>()
			{
				Result = results,
				IsSuccessful = true,
				Message = StatusLabels.GetSuccess("Revenue Collection")
			};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to get Revenue Collection");
			return new ApiResult<RevenueCollection>() { Message = "Failed to get Revenue Collection" };
		}
	}

	public async ValueTask<ApiResult> UpdateAsync(Guid id, RevenueCollectionManipulationDto model,
		CancellationToken cancellationToken)
	{
		try
		{
			var existingEntity = await _revenueCollectionRepository.GetById(id, cancellationToken);
			if (existingEntity == null)
			{
				Log.Error("{Revenue CollectionId} not found", id);
				return new ApiResult() { Message = "Revenue Collection not found" };
			}
			_mapper.Map(model, existingEntity);
			// _revenueCollectionRepository.Delete(existingEntity);
			_revenueCollectionRepository.Update(existingEntity);
			await _revenueCollectionRepository.SaveChangesAsync(cancellationToken);
			// _ = await CreateAsync(model, cancellationToken);

			Log.Information("Revenue Collection with ID:{Revenue CollectionId} was updated successfully", id);

			return new ApiResult() { IsSuccessful = true, Message = StatusLabels.UpdateSuccess("Revenue Collection") };
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to update Revenue Collection");
			return new ApiResult() { Message = "Failed to update Revenue Collection" };
		}
	}

	public async ValueTask<ApiResult> CreateAsync(RevenueCollectionManipulationDto model,
		CancellationToken cancellationToken)
	{
		try
		{
			var mappedEntity = _mapper.Map<RevenueCollection>(model);
			await _revenueCollectionRepository.InsertAsync(mappedEntity, cancellationToken);
			await _revenueCollectionRepository.SaveChangesAsync(cancellationToken);

			Log.Information("Revenue Collection with Id:{Revenue CollectionId} was saved successfully", mappedEntity.Id);

			return new ApiResult() { IsSuccessful = true, Message = StatusLabels.InsertSuccess("Revenue Collection") };
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to add Revenue Collection");
			return new ApiResult() { Message = "Failed to add Revenue Collection" };
		}
	}

	public async ValueTask<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
	{
		try
		{
			var existingEntity = await _revenueCollectionRepository.FindAsync(id);
			if (existingEntity == null)
			{
				Log.Error("{Revenue CollectionId} not found", id);
				return new ApiResult() { Message = "Revenue Collection not found" };
			}

			_revenueCollectionRepository.Delete(existingEntity);
			await _revenueCollectionRepository.SaveChangesAsync(cancellationToken);
			Log.Information("Revenue Collection with ID:{Revenue CollectionId} was deleted successfully", id);

			return new ApiResult() { IsSuccessful = true, Message = StatusLabels.DeleteSuccess("Revenue Collection") };
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to delete Revenue Collection");
			return new ApiResult() { Message = "Failed to delete Revenue Collection" };
		}
	}

	public async ValueTask<ApiResult> DeleteDetailAsync(Guid revenueCollectionDetailId, Guid revenueCollectionId, CancellationToken cancellationToken)
	{
		try
		{
			await _revenueCollectionRepository.DeleteRevenueCollectionDetail(revenueCollectionDetailId, revenueCollectionId, cancellationToken);
			await _revenueCollectionRepository.SaveChangesAsync(cancellationToken);
			Log.Information("Revenue Collection detail with ID:{Revenue CollectionId} was deleted successfully", revenueCollectionDetailId);

			return new ApiResult() { IsSuccessful = true, Message = StatusLabels.DeleteSuccess("Revenue Collection detail") };
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to delete Revenue Collection detail");
			return new ApiResult() { Message = "Failed to delete Revenue Collection detail" };
		}
	}


	public async Task<ApiResult> ExportRevenues(PagingParameters pagingParams, CancellationToken cancellationToken)
	{
		try
		{
			var result = await _revenueCollectionRepository.Export(pagingParams);
			Log.Information($"Revenue Collections exported successfully");

			await _jsRuntime.SaveAs(pagingParams.FileName + ".xlsx", result.ToArray());

			return new ApiResult()
			{
				IsSuccessful = true,
				Message = "Revenue Collection exported successfully",
			};
		}
		catch (Exception e)
		{
			Log.Fatal(e, e.Message);
			return new ApiResult()
			{
				Message = StatusLabels.ExportError
			};
		}
	}
}
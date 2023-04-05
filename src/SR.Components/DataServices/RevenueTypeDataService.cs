using GreatIdeas.Extensions;
using MapsterMapper;
using Serilog;
using SR.Data.Repositories.Contracts;
using SR.Shared;
using SR.Shared.Entities;

namespace SR.Components.DataServices;

public class RevenueTypeDataService : IRevenueTypeDataService
{
	private readonly IRevenueTypeRepository _permitTypeRepository;
	private IMapper _mapper = new Mapper();

	public RevenueTypeDataService(IRevenueTypeRepository permitTypeRepository)
	{
		_permitTypeRepository = permitTypeRepository;
	}

	public async ValueTask<ApiResults<RevenueType>> GetAllAsync(CancellationToken cancellationToken)
	{
		try
		{
			var results = await _permitTypeRepository.GetAllAsync(cancellationToken);
			return new ApiResults<RevenueType>()
			{
				Results = results, IsSuccessful = true, Message = StatusLabels.GetSuccess("Revenue Types")
			};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to get revenue types");
			return new ApiResults<RevenueType>() {Message = "Failed to get revenue types"};
		}
	}

	public async ValueTask<ApiResult<RevenueType>> GetByIdAsync(int id, CancellationToken cancellationToken)
	{
		try
		{
			var results = await _permitTypeRepository.GetFirstOrDefaultAsync(
				x=>x.Id == id, cancellationToken: cancellationToken);
			return new ApiResult<RevenueType>()
			{
				Result = results, IsSuccessful = true, Message = StatusLabels.GetSuccess("Revenue Type")
			};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to get revenue type");
			return new ApiResult<RevenueType>() {Message = "Failed to get revenue type"};
		}
	}

	public async ValueTask<ApiResult> UpdateAsync(int id, RevenueType model,
		CancellationToken cancellationToken)
	{
		try
		{
			var existingEntity = await _permitTypeRepository.FindAsync(id);
			if (existingEntity == null)
			{
				Log.Error("{RevenueTypeId} not found", id);
				return new ApiResult() {Message = "Revenue Type not found"};
			}
			_mapper.Map(model, existingEntity);
			_permitTypeRepository.UpdateDto(existingEntity);
			await _permitTypeRepository.SaveChangesAsync(cancellationToken);
			
			Log.Information("RevenueType with ID:{RevenueTypeId} was updated successfully", id);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.UpdateSuccess("Revenue Type")};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to update revenue type");
			return new ApiResult() {Message = "Failed to update revenue type"};
		}
	}

	public async ValueTask<ApiResult> CreateAsync(RevenueType model,
		CancellationToken cancellationToken)
	{
		try
		{
			var mappedEntity = _mapper.Map<RevenueType>(model);
			await _permitTypeRepository.InsertAsync(mappedEntity, cancellationToken);
			await _permitTypeRepository.SaveChangesAsync(cancellationToken);
			
			Log.Information("RevenueType with name:{RevenueTypeId} was saved successfully", model.Name);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.InsertSuccess("Revenue Type")};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to add revenue type");
			return new ApiResult() {Message = "Failed to add revenue type"};
		}
	}
	
	public async ValueTask<ApiResult> DeleteAsync(int id, CancellationToken cancellationToken)
	{
		try
		{
			var existingEntity = await _permitTypeRepository.FindAsync(id);
			if (existingEntity == null)
			{
				Log.Error("{RevenueTypeId} not found", id);
				return new ApiResult() {Message = "Revenue Type not found"};
			}
			
			_permitTypeRepository.Delete(existingEntity);
			await _permitTypeRepository.SaveChangesAsync(cancellationToken);
			Log.Information("RevenueType with ID:{RevenueTypeId} was deleted successfully", id);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.DeleteSuccess("Revenue Type")};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to delete revenue type");
			return new ApiResult() {Message = "Failed to delete revenue type"};
		}
	}
}
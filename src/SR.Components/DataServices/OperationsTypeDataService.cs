using GreatIdeas.Extensions;
using MapsterMapper;
using Serilog;
using SR.Data.Repositories.Contracts;
using SR.Shared;
using SR.Shared.Entities;

namespace SR.Components.DataServices;

public class OperationTypeDataService : IOperationTypeDataService
{
	private readonly IOperationTypeRepository _permitTypeRepository;
	private IMapper _mapper = new Mapper();

	public OperationTypeDataService(IOperationTypeRepository permitTypeRepository)
	{
		_permitTypeRepository = permitTypeRepository;
	}

	public async ValueTask<ApiResults<OperationType>> GetAllAsync(CancellationToken cancellationToken)
	{
		try
		{
			var results = await _permitTypeRepository.GetAllAsync(cancellationToken);
			return new ApiResults<OperationType>()
			{
				Results = results, IsSuccessful = true, Message = StatusLabels.GetSuccess("Operation Types")
			};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to get operation types");
			return new ApiResults<OperationType>() {Message = "Failed to get operation types"};
		}
	}

	public async ValueTask<ApiResult<OperationType>> GetByIdAsync(int id, CancellationToken cancellationToken)
	{
		try
		{
			var results = await _permitTypeRepository.GetFirstOrDefaultAsync(
				x=>x.Id == id, cancellationToken: cancellationToken);
			return new ApiResult<OperationType>()
			{
				Result = results, IsSuccessful = true, Message = StatusLabels.GetSuccess("Operation Type")
			};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to get operation type");
			return new ApiResult<OperationType>() {Message = "Failed to get operation type"};
		}
	}

	public async ValueTask<ApiResult> UpdateAsync(int id, OperationType model,
		CancellationToken cancellationToken)
	{
		try
		{
			var existingEntity = await _permitTypeRepository.FindAsync(id);
			if (existingEntity == null)
			{
				Log.Error("{OperationTypeId} not found", id);
				return new ApiResult() {Message = "Operation Type not found"};
			}
			_mapper.Map(model, existingEntity);
			_permitTypeRepository.UpdateDto(existingEntity);
			await _permitTypeRepository.SaveChangesAsync(cancellationToken);
			
			Log.Information("OperationType with ID:{OperationTypeId} was updated successfully", id);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.UpdateSuccess("Operation Type")};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to update operation type");
			return new ApiResult() {Message = "Failed to update operation type"};
		}
	}

	public async ValueTask<ApiResult> CreateAsync(OperationType model,
		CancellationToken cancellationToken)
	{
		try
		{
			var mappedEntity = _mapper.Map<OperationType>(model);
			await _permitTypeRepository.InsertAsync(mappedEntity, cancellationToken);
			await _permitTypeRepository.SaveChangesAsync(cancellationToken);
			
			Log.Information("OperationType with name:{OperationTypeId} was saved successfully", model.Name);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.InsertSuccess("Operation Type")};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to add operation type");
			return new ApiResult() {Message = "Failed to add operation type"};
		}
	}
	
	public async ValueTask<ApiResult> DeleteAsync(int id, CancellationToken cancellationToken)
	{
		try
		{
			var existingEntity = await _permitTypeRepository.FindAsync(id);
			if (existingEntity == null)
			{
				Log.Error("{OperationTypeId} not found", id);
				return new ApiResult() {Message = "Operation Type not found"};
			}
			
			_permitTypeRepository.Delete(existingEntity);
			await _permitTypeRepository.SaveChangesAsync(cancellationToken);
			Log.Information("OperationType with ID:{OperationTypeId} was deleted successfully", id);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.DeleteSuccess("Operation Type")};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to delete operation type");
			return new ApiResult() {Message = "Failed to delete operation type"};
		}
	}
}
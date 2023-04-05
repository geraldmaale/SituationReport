using GreatIdeas.Extensions;
using MapsterMapper;
using Serilog;
using SR.Data.Repositories.Contracts;
using SR.Shared;
using SR.Shared.DTOs.Operations;
using SR.Shared.Entities;

namespace SR.Components.DataServices;

public class OperationOfficeDataService : IOperationOfficeDataService
{
	private readonly IOperationOfficeRepository _operationOfficeOfficeRepository;
	private IMapper _mapper = new Mapper();

	public OperationOfficeDataService(IOperationOfficeRepository operationOfficeOfficeRepository)
	{
		_operationOfficeOfficeRepository = operationOfficeOfficeRepository;
	}

	public async ValueTask<ApiResults<OperationOfficeDto>> GetAllAsync(CancellationToken cancellationToken)
	{
		try
		{
			var results = await _operationOfficeOfficeRepository.GetAllProjectToAsync(cancellationToken);
			return new ApiResults<OperationOfficeDto>()
			{
				Results = results, IsSuccessful = true, Message = StatusLabels.GetSuccess("operation Offices")
			};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to get operation offices");
			return new ApiResults<OperationOfficeDto>() {Message = "Failed to get operation offices"};
		}
	}

	public async ValueTask<ApiResult<OperationOffice>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
	{
		try
		{
			var results = await _operationOfficeOfficeRepository.GetFirstOrDefaultAsync(
				x=>x.OperationOfficeId == id, cancellationToken: cancellationToken);
			return new ApiResult<OperationOffice>()
			{
				Result = results, IsSuccessful = true, Message = StatusLabels.GetSuccess("Operation Office")
			};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to get operation Office");
			return new ApiResult<OperationOffice>() {Message = "Failed to get operation Office"};
		}
	}

	public async ValueTask<ApiResult<OperationOfficeDto>> GetByIdDtoAsync(Guid id, CancellationToken cancellationToken)
	{
		try
		{
			var results = await _operationOfficeOfficeRepository.GetWithProjectToAsync(
				x=>x.OperationOfficeId == id, cancellationToken: cancellationToken);
			return new ApiResult<OperationOfficeDto>()
			{
				Result = results, IsSuccessful = true, Message = StatusLabels.GetSuccess("Operation Office")
			};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to get operation Office");
			return new ApiResult<OperationOfficeDto>() {Message = "Failed to get operation Office"};
		}
	}

	public async ValueTask<ApiResult> UpdateAsync(Guid id, OperationOffice model,
		CancellationToken cancellationToken)
	{
		try
		{
			var existingEntity = await _operationOfficeOfficeRepository.FindAsync(id);
			if (existingEntity == null)
			{
				Log.Error("{OperationOfficeId} not found", id);
				return new ApiResult() {Message = "Operation Office not found"};
			}
			_mapper.Map(model, existingEntity);
			_operationOfficeOfficeRepository.UpdateDto(existingEntity);
			await _operationOfficeOfficeRepository.SaveChangesAsync(cancellationToken);
			
			Log.Information("OperationOffice with ID:{OperationOfficeId} was updated successfully", id);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.UpdateSuccess("Operation Office")};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to update operation Office");
			return new ApiResult() {Message = "Failed to update operation Office"};
		}
	}

	public async ValueTask<ApiResult> CreateAsync(OperationOffice model,
		CancellationToken cancellationToken)
	{
		try
		{
			var mappedEntity = _mapper.Map<OperationOffice>(model);
			await _operationOfficeOfficeRepository.InsertAsync(mappedEntity, cancellationToken);
			await _operationOfficeOfficeRepository.SaveChangesAsync(cancellationToken);
			
			Log.Information("OperationOffice with name:{OperationOfficeId} was saved successfully", model.OfficeName);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.InsertSuccess("Operation Office")};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to add operation Office");
			return new ApiResult() {Message = "Failed to add operation Office"};
		}
	}
	
	public async ValueTask<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
	{
		try
		{
			var existingEntity = await _operationOfficeOfficeRepository.FindAsync(id);
			if (existingEntity == null)
			{
				Log.Error("{OperationOfficeId} not found", id);
				return new ApiResult() {Message = "Operation Office not found"};
			}
			
			_operationOfficeOfficeRepository.Delete(existingEntity);
			await _operationOfficeOfficeRepository.SaveChangesAsync(cancellationToken);
			Log.Information("OperationOffice with ID:{OperationOfficeId} was deleted successfully", id);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.DeleteSuccess("Operation Office")};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to delete operation Office");
			return new ApiResult() {Message = "Failed to delete operation Office"};
		}
	}
}
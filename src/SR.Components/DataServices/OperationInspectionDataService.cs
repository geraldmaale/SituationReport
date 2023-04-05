using GreatIdeas.Extensions;
using MapsterMapper;
using Serilog;
using SR.Data.Repositories.Contracts;
using SR.Shared;
using SR.Shared.DTOs.Operations;
using SR.Shared.Entities;

namespace SR.Components.DataServices;

public class OperationInspectionDataService : IOperationInspectionDataService
{
	private readonly IOperationProcessingRepository _passportProcessingRepository;
	private IMapper _mapper = new Mapper();

	public OperationInspectionDataService(IOperationProcessingRepository passportProcessingRepository)
	{
		_passportProcessingRepository = passportProcessingRepository;
	}

	public async ValueTask<ApiResults<OperationInspectionDto>> GetAllAsync(Guid operationOfficeId, CancellationToken cancellationToken)
	{
		try
		{
			var results = await _passportProcessingRepository.GetAllAsync(operationOfficeId, cancellationToken);
			return new ApiResults<OperationInspectionDto>()
			{
				Results = results, IsSuccessful = true, Message = StatusLabels.GetSuccess("Operation Inspection")
			};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to get Operation processing");
			return new ApiResults<OperationInspectionDto>() {Message = "Failed to get Operation processing"};
		}
	}

	public async ValueTask<ApiResult<OperationInspection>> GetByIdAsync(Guid operationOfficeId, Guid id, CancellationToken cancellationToken)
	{
		try
		{
			var results = await _passportProcessingRepository.GetById(operationOfficeId, id, cancellationToken);
			return new ApiResult<OperationInspection>()
			{
				Result = results, IsSuccessful = true, Message = StatusLabels.GetSuccess("Operation Inspection")
			};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to get Operation Inspection");
			return new ApiResult<OperationInspection>() {Message = "Failed to get Operation Inspection"};
		}
	}

	public async ValueTask<ApiResult> UpdateAsync(Guid operationOfficeId, Guid id, 
		OperationInspectionManipulationDto model, CancellationToken cancellationToken)
	{
		try
		{
			var existingEntity = await _passportProcessingRepository.GetById(operationOfficeId, id, cancellationToken);
			if (existingEntity == null)
			{
				Log.Error("{OperationProcessingId} not found", id);
				return new ApiResult() {Message = "Operation Inspection not found"};
			}
			_mapper.Map(model, existingEntity);
			
			// Delete existing records
			_passportProcessingRepository.Delete(existingEntity);
			await _passportProcessingRepository.SaveChangesAsync(cancellationToken);
			
			// Add new records
			await CreateAsync(operationOfficeId, model, cancellationToken);
			Log.Information("OperationProcessing with ID:{OperationProcessingId} was updated successfully", id);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.UpdateSuccess("Operation Inspection")};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to update Operation Inspection");
			return new ApiResult() {Message = "Failed to update Operation Inspection"};
		}
	}

	public async ValueTask<ApiResult> CreateAsync(Guid operationOfficeId, OperationInspectionManipulationDto model,
		CancellationToken cancellationToken)
	{
		try
		{
			var mappedEntity = _mapper.Map<OperationInspection>(model);
			await _passportProcessingRepository.InsertAsync(mappedEntity, cancellationToken);
			await _passportProcessingRepository.SaveChangesAsync(cancellationToken);
			
			Log.Information("OperationProcessing with Id:{OperationProcessingId} was saved successfully", mappedEntity.Id);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.InsertSuccess("Operation Inspection")};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to add Operation Inspection");
			return new ApiResult() {Message = "Failed to add Operation Inspection"};
		}
	}
	
	public async ValueTask<ApiResult> DeleteAsync(Guid operationOfficeId, Guid id, CancellationToken cancellationToken)
	{
		try
		{
			var existingEntity = await _passportProcessingRepository.FindAsync(id);
			if (existingEntity == null)
			{
				Log.Error("{OperationProcessingId} not found", id);
				return new ApiResult() {Message = "Operation Inspection not found"};
			}
			
			_passportProcessingRepository.Delete(existingEntity);
			await _passportProcessingRepository.SaveChangesAsync(cancellationToken);
			Log.Information("OperationProcessing with ID:{OperationProcessingId} was deleted successfully", id);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.DeleteSuccess("Operation Inspection")};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to delete Operation Inspection");
			return new ApiResult() {Message = "Failed to delete Operation Inspection"};
		}
	}
}
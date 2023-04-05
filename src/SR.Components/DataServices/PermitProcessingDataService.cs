using GreatIdeas.Extensions;
using MapsterMapper;
using Serilog;
using SR.Data.Repositories.Contracts;
using SR.Shared;
using SR.Shared.DTOs.Permits;
using SR.Shared.Entities;

namespace SR.Components.DataServices;

public class PermitProcessingDataService : IPermitProcessingDataService
{
	private readonly IPermitProcessingRepository _permitProcessingRepository;
	private IMapper _mapper = new Mapper();

	public PermitProcessingDataService(IPermitProcessingRepository crewProcessingRepository)
	{
		_permitProcessingRepository = crewProcessingRepository;
	}

	public async ValueTask<ApiResults<PermitProcessingDto>> GetAllAsync(CancellationToken cancellationToken)
	{
		try
		{
			var results = await _permitProcessingRepository.GetAllProjectToAsync(cancellationToken);
			return new ApiResults<PermitProcessingDto>()
			{
				Results = results.OrderByDescending(x=>x.EntryDate), 
				IsSuccessful = true, 
				Message = StatusLabels.GetSuccess("Permit Processings")
			};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to get Permit Processing");
			return new ApiResults<PermitProcessingDto>() {Message = "Failed to get Permit Processing"};
		}
	}

	public async ValueTask<ApiResult<PermitProcessing>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
	{
		try
		{
			var results = await _permitProcessingRepository.GetById(id, cancellationToken);
			return new ApiResult<PermitProcessing>()
			{
				Result = results, IsSuccessful = true, Message = StatusLabels.GetSuccess("Permit Unit")
			};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to get Permit Unit");
			return new ApiResult<PermitProcessing>() {Message = "Failed to get Permit Unit"};
		}
	}

	public async ValueTask<ApiResult> UpdateAsync(Guid id, PermitProcessingManipulationDto model,
		CancellationToken cancellationToken)
	{
		try
		{
			var existingEntity = await _permitProcessingRepository.GetById(id, cancellationToken);
			if (existingEntity == null)
			{
				Log.Error("{Permit ProcessingId} not found", id);
				return new ApiResult() {Message = "Permit Processing not found"};
			}
			_mapper.Map(model, existingEntity);
			_permitProcessingRepository.Update(existingEntity);
			await _permitProcessingRepository.SaveChangesAsync(cancellationToken);
			
			Log.Information("Permit Processing with ID:{Permit ProcessingId} was updated successfully", id);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.UpdateSuccess("Permit Processing")};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to update Permit Processing");
			return new ApiResult() {Message = "Failed to update Permit Processing"};
		}
	}

	public async ValueTask<ApiResult> CreateAsync(PermitProcessingManipulationDto model,
		CancellationToken cancellationToken)
	{
		try
		{
			var mappedEntity = _mapper.Map<PermitProcessing>(model);
			await _permitProcessingRepository.InsertAsync(mappedEntity, cancellationToken);
			await _permitProcessingRepository.SaveChangesAsync(cancellationToken);
			
			Log.Information("Permit Processing with Id:{Permit ProcessingId} was saved successfully", mappedEntity.Id);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.InsertSuccess("Permit Processing")};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to add Permit Processing");
			return new ApiResult() {Message = "Failed to add Permit Processing"};
		}
	}
	
	public async ValueTask<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
	{
		try
		{
			var existingEntity = await _permitProcessingRepository.FindAsync(id);
			if (existingEntity == null)
			{
				Log.Error("{Permit ProcessingId} not found", id);
				return new ApiResult() {Message = "Permit Processing not found"};
			}
			
			_permitProcessingRepository.Delete(existingEntity);
			await _permitProcessingRepository.SaveChangesAsync(cancellationToken);
			Log.Information("Permit Processing with ID:{Permit ProcessingId} was deleted successfully", id);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.DeleteSuccess("Permit Processing")};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to delete Permit Processing");
			return new ApiResult() {Message = "Failed to delete Permit Processing"};
		}
	}

	public async ValueTask<ApiResult> DeleteDetailAsync(Guid permitProcessingDetailId, Guid permitProcessingId,
		CancellationToken cancellationToken)
	{
		try
		{
			await _permitProcessingRepository.DeletePermitDetail(permitProcessingDetailId, permitProcessingId,
				cancellationToken);
			Log.Information("Permit Processing detail with ID:{Permit ProcessingId} was deleted successfully", permitProcessingDetailId);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.DeleteSuccess("Permit Processing detail")};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to delete Permit Processing detail");
			return new ApiResult() {Message = "Failed to delete Permit Processing detail"};
		}
	}
}
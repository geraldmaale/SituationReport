using GreatIdeas.Extensions;
using MapsterMapper;
using Serilog;
using SR.Data.Repositories.Contracts;
using SR.Shared;
using SR.Shared.DTOs.Permits;
using SR.Shared.Entities;

namespace SR.Components.DataServices;

public class PermitTypeDataService : IPermitTypeDataService
{
	private readonly IPermitTypeRepo _permitTypeRepository;
	private IMapper _mapper = new Mapper();

	public PermitTypeDataService(IPermitTypeRepo permitTypeRepository)
	{
		_permitTypeRepository = permitTypeRepository;
	}

	public async ValueTask<ApiResults<PermitTypeDto>> GetAllAsync(CancellationToken cancellationToken)
	{
		try
		{
			var results = await _permitTypeRepository.GetAllProjectToAsync(cancellationToken);
			return new ApiResults<PermitTypeDto>()
			{
				Results = results, IsSuccessful = true, Message = StatusLabels.GetSuccess("Permit Types")
			};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to get permit types");
			return new ApiResults<PermitTypeDto>() {Message = "Failed to get permit types"};
		}
	}

	public async ValueTask<ApiResult<PermitTypeDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
	{
		try
		{
			var results = await _permitTypeRepository.GetWithProjectToAsync(
				x=>x.Id == id, cancellationToken);
			return new ApiResult<PermitTypeDto>()
			{
				Result = results, IsSuccessful = true, Message = StatusLabels.GetSuccess("Permit Type")
			};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to get permit type");
			return new ApiResult<PermitTypeDto>() {Message = "Failed to get permit type"};
		}
	}

	public async ValueTask<ApiResult> UpdateAsync(int id, PermitTypeManipulationDto model,
		CancellationToken cancellationToken)
	{
		try
		{
			var existingEntity = await _permitTypeRepository.FindAsync(id);
			if (existingEntity == null)
			{
				Log.Error("{PermitTypeId} not found", id);
				return new ApiResult() {Message = "Permit Type not found"};
			}
			_mapper.Map(model, existingEntity);
			_permitTypeRepository.UpdateDto(existingEntity);
			await _permitTypeRepository.SaveChangesAsync(cancellationToken);
			
			Log.Information("PermitType with ID:{PermitTypeId} was updated successfully", id);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.UpdateSuccess("Permit Type")};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to update permit type");
			return new ApiResult() {Message = "Failed to update permit type"};
		}
	}

	public async ValueTask<ApiResult> CreateAsync(PermitTypeManipulationDto model,
		CancellationToken cancellationToken)
	{
		try
		{
			var mappedEntity = _mapper.Map<PermitType>(model);
			await _permitTypeRepository.InsertAsync(mappedEntity, cancellationToken);
			await _permitTypeRepository.SaveChangesAsync(cancellationToken);
			
			Log.Information("PermitType with name:{PermitTypeId} was saved successfully", model.Name);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.InsertSuccess("Permit Type")};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to add permit type");
			return new ApiResult() {Message = "Failed to add permit type"};
		}
	}
	
	public async ValueTask<ApiResult> DeleteAsync(int id, CancellationToken cancellationToken)
	{
		try
		{
			var existingEntity = await _permitTypeRepository.FindAsync(id);
			if (existingEntity == null)
			{
				Log.Error("{PermitTypeId} not found", id);
				return new ApiResult() {Message = "Permit Type not found"};
			}
			
			_permitTypeRepository.Delete(existingEntity);
			await _permitTypeRepository.SaveChangesAsync(cancellationToken);
			Log.Information("PermitType with ID:{PermitTypeId} was deleted successfully", id);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.DeleteSuccess("Permit Type")};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to delete permit type");
			return new ApiResult() {Message = "Failed to delete permit type"};
		}
	}
}
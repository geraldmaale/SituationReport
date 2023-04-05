using GreatIdeas.Extensions;
using MapsterMapper;
using Serilog;
using SR.Data.Repositories.Contracts;
using SR.Shared;
using SR.Shared.Entities;

namespace SR.Components.DataServices;

public class VesselTypeDataService : IVesselTypeDataService
{
	private readonly IVesselTypeRepository _permitTypeRepository;
	private IMapper _mapper = new Mapper();

	public VesselTypeDataService(IVesselTypeRepository permitTypeRepository)
	{
		_permitTypeRepository = permitTypeRepository;
	}

	public async ValueTask<ApiResults<VesselType>> GetAllAsync(CancellationToken cancellationToken)
	{
		try
		{
			var results = await _permitTypeRepository.GetAllAsync(cancellationToken);
			return new ApiResults<VesselType>()
			{
				Results = results, IsSuccessful = true, Message = StatusLabels.GetSuccess("Vessel Types")
			};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to get vessel types");
			return new ApiResults<VesselType>() {Message = "Failed to get vessel types"};
		}
	}

	public async ValueTask<ApiResult<VesselType>> GetByIdAsync(int id, CancellationToken cancellationToken)
	{
		try
		{
			var results = await _permitTypeRepository.GetFirstOrDefaultAsync(
				x=>x.Id == id, cancellationToken: cancellationToken);
			return new ApiResult<VesselType>()
			{
				Result = results, IsSuccessful = true, Message = StatusLabels.GetSuccess("Vessel Type")
			};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to get vessel type");
			return new ApiResult<VesselType>() {Message = "Failed to get vessel type"};
		}
	}

	public async ValueTask<ApiResult> UpdateAsync(int id, VesselType model,
		CancellationToken cancellationToken)
	{
		try
		{
			var existingEntity = await _permitTypeRepository.FindAsync(id);
			if (existingEntity == null)
			{
				Log.Error("{VesselTypeId} not found", id);
				return new ApiResult() {Message = "Vessel Type not found"};
			}
			_mapper.Map(model, existingEntity);
			_permitTypeRepository.UpdateDto(existingEntity);
			await _permitTypeRepository.SaveChangesAsync(cancellationToken);
			
			Log.Information("VesselType with ID:{VesselTypeId} was updated successfully", id);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.UpdateSuccess("Vessel Type")};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to update vessel type");
			return new ApiResult() {Message = "Failed to update vessel type"};
		}
	}

	public async ValueTask<ApiResult> CreateAsync(VesselType model,
		CancellationToken cancellationToken)
	{
		try
		{
			var mappedEntity = _mapper.Map<VesselType>(model);
			await _permitTypeRepository.InsertAsync(mappedEntity, cancellationToken);
			await _permitTypeRepository.SaveChangesAsync(cancellationToken);
			
			Log.Information("VesselType with name:{VesselTypeId} was saved successfully", model.Name);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.InsertSuccess("Vessel Type")};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to add vessel type");
			return new ApiResult() {Message = "Failed to add vessel type"};
		}
	}
	
	public async ValueTask<ApiResult> DeleteAsync(int id, CancellationToken cancellationToken)
	{
		try
		{
			var existingEntity = await _permitTypeRepository.FindAsync(id);
			if (existingEntity == null)
			{
				Log.Error("{VesselTypeId} not found", id);
				return new ApiResult() {Message = "Vessel Type not found"};
			}
			
			_permitTypeRepository.Delete(existingEntity);
			await _permitTypeRepository.SaveChangesAsync(cancellationToken);
			Log.Information("VesselType with ID:{VesselTypeId} was deleted successfully", id);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.DeleteSuccess("Vessel Type")};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to delete vessel type");
			return new ApiResult() {Message = "Failed to delete vessel type"};
		}
	}
}
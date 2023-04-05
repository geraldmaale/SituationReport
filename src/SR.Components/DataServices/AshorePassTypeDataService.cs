using GreatIdeas.Extensions;
using MapsterMapper;
using Serilog;
using SR.Data.Repositories.Contracts;
using SR.Shared;
using SR.Shared.Entities;

namespace SR.Components.DataServices;

public class AshorePassTypeDataService : IAshorePassTypeDataService
{
	private readonly IAshorePassTypeRepository _permitTypeRepository;
	private IMapper _mapper = new Mapper();

	public AshorePassTypeDataService(IAshorePassTypeRepository permitTypeRepository)
	{
		_permitTypeRepository = permitTypeRepository;
	}

	public async ValueTask<ApiResults<AshorePassType>> GetAllAsync(CancellationToken cancellationToken)
	{
		try
		{
			var results = await _permitTypeRepository.GetAllAsync(cancellationToken);
			return new ApiResults<AshorePassType>()
			{
				Results = results, IsSuccessful = true, Message = StatusLabels.GetSuccess("Ashore Pass Types")
			};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to get vessel types");
			return new ApiResults<AshorePassType>() {Message = "Failed to get vessel types"};
		}
	}

	public async ValueTask<ApiResult<AshorePassType>> GetByIdAsync(int id, CancellationToken cancellationToken)
	{
		try
		{
			var results = await _permitTypeRepository.GetFirstOrDefaultAsync(
				x=>x.Id == id, cancellationToken: cancellationToken);
			return new ApiResult<AshorePassType>()
			{
				Result = results, IsSuccessful = true, Message = StatusLabels.GetSuccess("Ashore Pass Type")
			};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to get vessel type");
			return new ApiResult<AshorePassType>() {Message = "Failed to get vessel type"};
		}
	}

	public async ValueTask<ApiResult> UpdateAsync(int id, AshorePassType model,
		CancellationToken cancellationToken)
	{
		try
		{
			var existingEntity = await _permitTypeRepository.FindAsync(id);
			if (existingEntity == null)
			{
				Log.Error("{AshorePassTypeId} not found", id);
				return new ApiResult() {Message = "Ashore Pass Type not found"};
			}
			_mapper.Map(model, existingEntity);
			_permitTypeRepository.UpdateDto(existingEntity);
			await _permitTypeRepository.SaveChangesAsync(cancellationToken);
			
			Log.Information("AshorePassType with ID:{AshorePassTypeId} was updated successfully", id);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.UpdateSuccess("Ashore Pass Type")};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to update vessel type");
			return new ApiResult() {Message = "Failed to update vessel type"};
		}
	}

	public async ValueTask<ApiResult> CreateAsync(AshorePassType model,
		CancellationToken cancellationToken)
	{
		try
		{
			var mappedEntity = _mapper.Map<AshorePassType>(model);
			await _permitTypeRepository.InsertAsync(mappedEntity, cancellationToken);
			await _permitTypeRepository.SaveChangesAsync(cancellationToken);
			
			Log.Information("AshorePassType with name:{AshorePassTypeId} was saved successfully", model.Name);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.InsertSuccess("Ashore Pass Type")};
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
				Log.Error("{AshorePassTypeId} not found", id);
				return new ApiResult() {Message = "Ashore Pass Type not found"};
			}
			
			_permitTypeRepository.Delete(existingEntity);
			await _permitTypeRepository.SaveChangesAsync(cancellationToken);
			Log.Information("AshorePassType with ID:{AshorePassTypeId} was deleted successfully", id);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.DeleteSuccess("Ashore Pass Type")};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to delete vessel type");
			return new ApiResult() {Message = "Failed to delete vessel type"};
		}
	}
}
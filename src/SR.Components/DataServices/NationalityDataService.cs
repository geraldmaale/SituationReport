using GreatIdeas.Extensions;
using MapsterMapper;
using Serilog;
using SR.Data.Repositories.Contracts;
using SR.Shared;
using SR.Shared.Entities;

namespace SR.Components.DataServices;

public class NationalityTypeDataService : INationalityTypeDataService
{
	private readonly INationalityTypeRepository _permitTypeRepository;
	private IMapper _mapper = new Mapper();

	public NationalityTypeDataService(INationalityTypeRepository permitTypeRepository)
	{
		_permitTypeRepository = permitTypeRepository;
	}

	public async ValueTask<ApiResults<NationalityType>> GetAllAsync(CancellationToken cancellationToken)
	{
		try
		{
			var results = await _permitTypeRepository.GetAllAsync(cancellationToken);
			return new ApiResults<NationalityType>()
			{
				Results = results, IsSuccessful = true, Message = StatusLabels.GetSuccess("Nationality Types")
			};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to get nationality types");
			return new ApiResults<NationalityType>() {Message = "Failed to get nationality types"};
		}
	}

	public async ValueTask<ApiResult<NationalityType>> GetByIdAsync(int id, CancellationToken cancellationToken)
	{
		try
		{
			var results = await _permitTypeRepository.GetFirstOrDefaultAsync(
				x=>x.Id == id, cancellationToken: cancellationToken);
			return new ApiResult<NationalityType>()
			{
				Result = results, IsSuccessful = true, Message = StatusLabels.GetSuccess("Nationality Type")
			};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to get nationality type");
			return new ApiResult<NationalityType>() {Message = "Failed to get nationality type"};
		}
	}

	public async ValueTask<ApiResult> UpdateAsync(int id, NationalityType model,
		CancellationToken cancellationToken)
	{
		try
		{
			var existingEntity = await _permitTypeRepository.FindAsync(id);
			if (existingEntity == null)
			{
				Log.Error("{NationalityTypeId} not found", id);
				return new ApiResult() {Message = "Nationality Type not found"};
			}
			_mapper.Map(model, existingEntity);
			_permitTypeRepository.UpdateDto(existingEntity);
			await _permitTypeRepository.SaveChangesAsync(cancellationToken);
			
			Log.Information("NationalityType with ID:{NationalityTypeId} was updated successfully", id);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.UpdateSuccess("Nationality Type")};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to update nationality type");
			return new ApiResult() {Message = "Failed to update nationality type"};
		}
	}

	public async ValueTask<ApiResult> CreateAsync(NationalityType model,
		CancellationToken cancellationToken)
	{
		try
		{
			var mappedEntity = _mapper.Map<NationalityType>(model);
			await _permitTypeRepository.InsertAsync(mappedEntity, cancellationToken);
			await _permitTypeRepository.SaveChangesAsync(cancellationToken);
			
			Log.Information("NationalityType with name:{NationalityTypeId} was saved successfully", model.Name);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.InsertSuccess("Nationality Type")};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to add nationality type");
			return new ApiResult() {Message = "Failed to add nationality type"};
		}
	}
	
	public async ValueTask<ApiResult> DeleteAsync(int id, CancellationToken cancellationToken)
	{
		try
		{
			var existingEntity = await _permitTypeRepository.FindAsync(id);
			if (existingEntity == null)
			{
				Log.Error("{NationalityTypeId} not found", id);
				return new ApiResult() {Message = "Nationality Type not found"};
			}
			
			_permitTypeRepository.Delete(existingEntity);
			await _permitTypeRepository.SaveChangesAsync(cancellationToken);
			Log.Information("NationalityType with ID:{NationalityTypeId} was deleted successfully", id);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.DeleteSuccess("Nationality Type")};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to delete nationality type");
			return new ApiResult() {Message = "Failed to delete nationality type"};
		}
	}
}
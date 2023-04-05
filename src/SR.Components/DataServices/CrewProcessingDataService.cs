using GreatIdeas.Extensions;
using MapsterMapper;
using Serilog;
using SR.Data.Repositories.Contracts;
using SR.Shared;
using SR.Shared.DTOs.Crews;
using SR.Shared.Entities;

namespace SR.Components.DataServices;

public class CrewProcessingDataService : ICrewProcessingDataService
{
	private readonly ICrewProcessingRepository _crewProcessingRepository;
	private IMapper _mapper = new Mapper();

	public CrewProcessingDataService(ICrewProcessingRepository crewProcessingRepository)
	{
		_crewProcessingRepository = crewProcessingRepository;
	}

	public async ValueTask<ApiResults<CrewProcessingDto>> GetAllAsync(CancellationToken cancellationToken)
	{
		try
		{
			var results = await _crewProcessingRepository.GetAllProjectToAsync(cancellationToken);
			return new ApiResults<CrewProcessingDto>()
			{
				Results = results.OrderByDescending(x=>x.EntryDate), 
				IsSuccessful = true, 
				Message = StatusLabels.GetSuccess("CrewProcessings")
			};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to get CrewProcessing");
			return new ApiResults<CrewProcessingDto>() {Message = "Failed to get CrewProcessing"};
		}
	}

	public async ValueTask<ApiResult<CrewProcessing>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
	{
		try
		{
			var results = await _crewProcessingRepository.GetById(id, cancellationToken);
			return new ApiResult<CrewProcessing>()
			{
				Result = results, IsSuccessful = true, Message = StatusLabels.GetSuccess("Crew Processing")
			};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to get Crew Processing");
			return new ApiResult<CrewProcessing>() {Message = "Failed to get Crew Processing"};
		}
	}

	public async ValueTask<ApiResult> UpdateAsync(Guid id, CrewProcessingManipulationDto model,
		CancellationToken cancellationToken)
	{
		try
		{
			var existingEntity = await _crewProcessingRepository.GetById(id, cancellationToken);
			if (existingEntity is null)
			{
				Log.Error("{CrewProcessingId} not found", id);
				return new ApiResult() {Message = "CrewProcessing not found"};
			}
			
			_mapper.Map(model, existingEntity);
			_crewProcessingRepository.Update(existingEntity);
			await _crewProcessingRepository.SaveChangesAsync(cancellationToken);
			
			Log.Information("CrewProcessing with ID:{CrewProcessingId} was updated successfully", id);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.UpdateSuccess("CrewProcessing")};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to update CrewProcessing");
			return new ApiResult() {Message = "Failed to update CrewProcessing"};
		}
	}

	public async ValueTask<ApiResult> CreateAsync(CrewProcessingManipulationDto model,
		CancellationToken cancellationToken)
	{
		try
		{
			var mappedEntity = _mapper.Map<CrewProcessing>(model);
			await _crewProcessingRepository.InsertAsync(mappedEntity, cancellationToken);
			await _crewProcessingRepository.SaveChangesAsync(cancellationToken);
			
			Log.Information("CrewProcessing with Id:{CrewProcessingId} was saved successfully", mappedEntity.Id);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.InsertSuccess("CrewProcessing")};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to add CrewProcessing");
			return new ApiResult() {Message = "Failed to add CrewProcessing"};
		}
	}
	
	public async ValueTask<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
	{
		try
		{
			var existingEntity = await _crewProcessingRepository.FindAsync(id);
			if (existingEntity == null)
			{
				Log.Error("{CrewProcessingId} not found", id);
				return new ApiResult() {Message = "CrewProcessing not found"};
			}
			
			_crewProcessingRepository.Delete(existingEntity);
			await _crewProcessingRepository.SaveChangesAsync(cancellationToken);
			Log.Information("CrewProcessing with ID:{CrewProcessingId} was deleted successfully", id);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.DeleteSuccess("CrewProcessing")};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to delete CrewProcessing");
			return new ApiResult() {Message = "Failed to delete CrewProcessing"};
		}
	}
	
	public async ValueTask<ApiResult> DeleteEmbarkationAsync(Guid embarkationId, Guid crewProcessingId, CancellationToken cancellationToken)
	{
		try
		{
			await _crewProcessingRepository.DeleteEmbarkationDetail(embarkationId, crewProcessingId, cancellationToken);
			Log.Information("Embarkation with ID:{EmbarkationId} was deleted successfully", embarkationId);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.DeleteSuccess("Embarkation")};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to delete Embarkation");
			return new ApiResult() {Message = "Failed to delete Embarkation"};
		}
	}
	
	public async ValueTask<ApiResult> DeleteDisEmbarkationAsync(Guid disembarkationId, Guid crewProcessingId, CancellationToken cancellationToken)
	{
		try
		{
			await _crewProcessingRepository.DeleteDisEmbarkationDetail(disembarkationId, crewProcessingId, cancellationToken);
			Log.Information("DisEmbarkation with ID:{EmbarkationId} was deleted successfully", disembarkationId);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.DeleteSuccess("DisEmbarkation")};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to delete DisEmbarkation");
			return new ApiResult() {Message = "Failed to delete DisEmbarkation"};
		}
	}
	
	public async ValueTask<ApiResult> DeleteVesselDockedAsync(Guid vesselDockedId, Guid crewProcessingId, CancellationToken cancellationToken)
	{
		try
		{
			await _crewProcessingRepository.DeleteDisEmbarkationDetail(vesselDockedId, crewProcessingId, cancellationToken);
			Log.Information("VesselDocked with ID:{EmbarkationId} was deleted successfully", vesselDockedId);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.DeleteSuccess("Vessel Docked")};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to delete VesselDocked");
			return new ApiResult() {Message = "Failed to delete Vessel Docked"};
		}
	}

}
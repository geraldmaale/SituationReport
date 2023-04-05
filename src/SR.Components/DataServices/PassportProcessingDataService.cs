using GreatIdeas.Extensions;
using MapsterMapper;
using Serilog;
using SR.Data.Repositories.Contracts;
using SR.Shared;
using SR.Shared.DTOs.Passports;
using SR.Shared.Entities;

namespace SR.Components.DataServices;

public class PassportProcessingDataService : IPassportProcessingDataService
{
	private readonly IPassportProcessingRepository _passportProcessingRepository;
	private IMapper _mapper = new Mapper();

	public PassportProcessingDataService(IPassportProcessingRepository passportProcessingRepository)
	{
		_passportProcessingRepository = passportProcessingRepository;
	}

	public async ValueTask<ApiResults<PassportProcessingDto>> GetAllAsync(CancellationToken cancellationToken)
	{
		try
		{
			var results = await _passportProcessingRepository.GetAllProjectToAsync(cancellationToken);
			return new ApiResults<PassportProcessingDto>()
			{
				Results = results.OrderByDescending(x=>x.EntryDate), 
				IsSuccessful = true, 
				Message = StatusLabels.GetSuccess("Passport Processing")
			};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to get passport processing");
			return new ApiResults<PassportProcessingDto>() {Message = "Failed to get passport processing"};
		}
	}

	public async ValueTask<ApiResult<PassportProcessing>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
	{
		try
		{
			var results = await _passportProcessingRepository.GetById(id, cancellationToken);
			return new ApiResult<PassportProcessing>()
			{
				Result = results, IsSuccessful = true, Message = StatusLabels.GetSuccess("Passport Processing")
			};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to get Passport Processing");
			return new ApiResult<PassportProcessing>() {Message = "Failed to get Passport Processing"};
		}
	}

	public async ValueTask<ApiResult> UpdateAsync(Guid id, PassportProcessingManipulationDto model,
		CancellationToken cancellationToken)
	{
		try
		{
			var existingEntity = await _passportProcessingRepository.FindAsync(id);
			if (existingEntity == null)
			{
				Log.Error("{PassportProcessingId} not found", id);
				return new ApiResult() {Message = "Passport Processing not found"};
			}
			_mapper.Map(model, existingEntity);
			_passportProcessingRepository.UpdateDto(existingEntity);
			await _passportProcessingRepository.SaveChangesAsync(cancellationToken);
			
			Log.Information("PassportProcessing with ID:{PassportProcessingId} was updated successfully", id);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.UpdateSuccess("Passport Processing")};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to update Passport Processing");
			return new ApiResult() {Message = "Failed to update Passport Processing"};
		}
	}

	public async ValueTask<ApiResult> CreateAsync(PassportProcessingManipulationDto model,
		CancellationToken cancellationToken)
	{
		try
		{
			var mappedEntity = _mapper.Map<PassportProcessing>(model);
			await _passportProcessingRepository.InsertAsync(mappedEntity, cancellationToken);
			await _passportProcessingRepository.SaveChangesAsync(cancellationToken);
			
			Log.Information("PassportProcessing with Id:{PassportProcessingId} was saved successfully", mappedEntity.Id);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.InsertSuccess("Passport Processing")};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to add Passport Processing");
			return new ApiResult() {Message = "Failed to add Passport Processing"};
		}
	}
	
	public async ValueTask<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
	{
		try
		{
			var existingEntity = await _passportProcessingRepository.FindAsync(id);
			if (existingEntity == null)
			{
				Log.Error("{PassportProcessingId} not found", id);
				return new ApiResult() {Message = "Passport Processing not found"};
			}
			
			_passportProcessingRepository.Delete(existingEntity);
			await _passportProcessingRepository.SaveChangesAsync(cancellationToken);
			Log.Information("PassportProcessing with ID:{PassportProcessingId} was deleted successfully", id);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.DeleteSuccess("Passport Processing")};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to delete Passport Processing");
			return new ApiResult() {Message = "Failed to delete Passport Processing"};
		}
	}
}
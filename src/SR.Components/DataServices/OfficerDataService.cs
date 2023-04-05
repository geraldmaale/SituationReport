using GreatIdeas.Extensions;
using MapsterMapper;
using Serilog;
using SR.Data.Repositories.Contracts;
using SR.Shared;
using SR.Shared.DTOs.Officers;
using SR.Shared.DTOs.User;
using SR.Shared.Entities;

namespace SR.Components.DataServices;

public class OfficerDataService : IOfficerDataService
{
	private readonly IOfficerRepository _officerRepository;
	private readonly IUserRepository _userRepository;
	private IMapper _mapper = new Mapper();

	public OfficerDataService(IOfficerRepository officerRepository, IUserRepository userRepository)
	{
		_officerRepository = officerRepository;
		_userRepository = userRepository;
	}

	public async ValueTask<ApiResults<OfficerDto>> GetAllAsync(CancellationToken cancellationToken)
	{
		try
		{
			var results = await _officerRepository.GetAllProjectToAsync(cancellationToken);
			return new ApiResults<OfficerDto>()
			{
				Results = results.OrderBy(x=>x.FullName), 
				IsSuccessful = true, 
				Message = StatusLabels.GetSuccess("Officers")
			};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to get officer");
			return new ApiResults<OfficerDto>() {Message = "Failed to get officer"};
		}
	}

	public async ValueTask<ApiResult<OfficerFullDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
	{
		try
		{
			var results = await _officerRepository.GetById(id, cancellationToken);
			return new ApiResult<OfficerFullDto>()
			{
				Result = results, IsSuccessful = true, Message = StatusLabels.GetSuccess("Officer")
			};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to get officer");
			return new ApiResult<OfficerFullDto>() {Message = "Failed to get officer"};
		}
	}

	public async ValueTask<ApiResult> UpdateAsync(Guid id, OfficerUpdateDto model,
		CancellationToken cancellationToken)
	{
		try
		{
			var existingEntity = await _officerRepository.FindAsync(id);
			if (existingEntity == null)
			{
				Log.Error("{OfficerId} not found", id);
				return new ApiResult() {Message = "Officer not found"};
			}

			if (string.IsNullOrWhiteSpace(existingEntity.Username) && !string.IsNullOrWhiteSpace(model.Username))
			{
				var user = new UserCreationDto()
				{
					UserName = model.Username,
					FullName = model.FullName,
					Email = model.Username,
					Password = model.Password,
					ConfirmPassword = model.Password,
					PhoneNumber = model.PhoneNumber,
					OfficerId = id
				};
				await _userRepository.CreateUserAsync(user);
				model.Email = model.Username;
			}
			
			_mapper.Map(model, existingEntity);
			_officerRepository.UpdateDto(existingEntity);
			await _officerRepository.SaveChangesAsync(cancellationToken);
			
			Log.Information("Officer with ID:{OfficerId} was updated successfully", id);

			return new ApiResult() {IsSuccessful = true, Message = StatusLabels.UpdateSuccess("Officer")};
		}
		catch (ApplicationException e)
		{
			return new ApiResult() {Message = e.Message};
		}
		catch (Exception e)
		{
			Log.Fatal(e, "Failed to update officer");
			return new ApiResult() {Message = "Failed to update officer"};
		}
	}

	public async ValueTask<ApiResult<OfficerDto>> CreateAsync(OfficerCreationDto model,
		CancellationToken cancellationToken)
	{
		try
		{
			var newOfficer = await _officerRepository.CreateOfficer(model, cancellationToken);
			
			Log.Information("Officer with Id:{OfficerId} was saved successfully", newOfficer.OfficerId);

			return new ApiResult<OfficerDto>()
			{
				IsSuccessful = true,
				Result = newOfficer,
				Message = StatusLabels.InsertSuccess("Officer")
			};
		}
		catch (ApplicationException e)
		{
			return new ApiResult<OfficerDto>() {Message = e.Message};
		}
		catch (Exception)
		{
			return new ApiResult<OfficerDto>() {Message = "Failed to add officer"};
		}
	}
	
	public async ValueTask<ApiResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
	{
		try
		{
			var result = await _officerRepository.DeleteOfficer(id, cancellationToken);
			if (result)
			{
				Log.Information("Officer with ID:{OfficerId} was deleted successfully", id);

				return new ApiResult() {IsSuccessful = true, Message = StatusLabels.DeleteSuccess("Officer")};
			}
			
			Log.Information("Could not delete Officer with ID:{OfficerId}", id);
			return new ApiResult() {IsSuccessful = false, Message = StatusLabels.UnprocessableDeleteError};
		}
		catch (ApplicationException e)
		{
			return new ApiResult() {Message = e.Message};
		}
		catch (Exception e)
		{
			return new ApiResult() {Message = e.Message};
		}
	}
}
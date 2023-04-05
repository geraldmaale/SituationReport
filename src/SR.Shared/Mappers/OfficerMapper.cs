using GreatIdeas.Extensions;
using Mapster;
using SR.Shared.DTOs.Officers;
using SR.Shared.Entities;

namespace SR.Shared.Mappers;
public class OfficerMapper: IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<Officer, OfficerCreationDto>()
			.GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);
		
		config.ForType<OfficerFullDto, OfficerCreationDto>()
			.GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);

		config.ForType<OfficerDto, OfficerCreationDto>()
			.GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);
		
		config.ForType<Officer, OfficerUpdateDto>()
			.GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);

		config.ForType<OfficerDto, OfficerUpdateDto>()
			.GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);

		config.ForType<Officer, OfficerDto>()
			.Map(src => src.UserId, dest => dest.User.Id)
			.Map(src => src.Rank, dest => dest.Rank.GetDescription())
			.GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);
	}
}

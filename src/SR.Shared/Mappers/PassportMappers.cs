using Mapster;
using SR.Shared.DTOs.Officers;
using SR.Shared.DTOs.Passports;
using SR.Shared.DTOs.Permits;
using SR.Shared.Entities;

namespace SR.Shared.Mappers;
public class PassportMappers: IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<PassportProcessing, PassportProcessingDto>()
			.Map(src=>src.Officer,dest=>dest.Officer.FullName)
			.GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);

		config.ForType<PassportProcessingDto, PassportProcessingManipulationDto>()
			.GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);

		config.ForType<PassportProcessing, PassportProcessingManipulationDto>()
			.GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);
	}
}

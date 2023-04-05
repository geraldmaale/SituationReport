using Mapster;
using SR.Shared.DTOs.Officers;
using SR.Shared.DTOs.Permits;
using SR.Shared.Entities;

namespace SR.Shared.Mappers;
public class PermitMappers: IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<PermitType, PermitTypeDto>()
			.GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);

		config.ForType<PermitTypeDto, PermitTypeManipulationDto>()
			.GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);

		config.ForType<PermitType, PermitTypeManipulationDto>()
			.GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);
		
		config.ForType<PermitProcessing, PermitProcessingDto>()
			.Map(src=>src.OfficerName,dest=>dest.Officer.FullName)
			.GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);
		
		config.ForType<PermitProcessingDto, PermitProcessingManipulationDto>()
			.GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);

		config.ForType<PermitProcessing, PermitProcessingManipulationDto>()
			.GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);
		
		config.ForType<PermitProcessingDetail, PermitProcessingDetailDto>()
			.Map(src=>src.Nationality,dest=>dest.Nationality.Name)
			.Map(src=>src.PermitType,dest=>dest.PermitType.Name)
			.GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);
		
		config.ForType<PermitProcessingDetailDto, PermitProcessingDetailManipulationDto>()
			.GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);

		config.ForType<PermitProcessingDetail, PermitProcessingDetailManipulationDto>()
			.GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);

	}
}

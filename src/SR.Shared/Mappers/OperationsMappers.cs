using Mapster;
using SR.Shared.DTOs.Operations;
using SR.Shared.DTOs.Revenues;
using SR.Shared.Entities;

namespace SR.Shared.Mappers;
public class OperationsMappers: IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<OperationOffice, OperationOfficeDto>()
			.GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);
		
		config.ForType<OperationInspection, OperationInspectionDto>()
			.Map(src=>src.OfficerName,dest=>dest.Officer.FullName)
			.GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);

		config.ForType<OperationInspection, OperationInspectionManipulationDto>()
			.GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);

		config.ForType<OperationInspectionDto, OperationInspectionManipulationDto>()
			.GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);
		
		config.ForType<OperationInspectionDetail, OperationInspectionDetailDto>()
			.Map(src=>src.OperationTypeName,dest=>dest.OperationType.Name)
			.GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);
	}
}

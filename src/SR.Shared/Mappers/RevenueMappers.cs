using Mapster;
using SR.Shared.DTOs.Revenues;
using SR.Shared.Entities;

namespace SR.Shared.Mappers;
public class RevenueMappers: IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<RevenueCollection, RevenueCollectionDto>()
			.Map(src=>src.Officer,dest=>dest.Officer.FullName)
			.GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);

		config.ForType<RevenueCollectionDto, RevenueCollectionManipulationDto>()
			.GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);

		config.ForType<RevenueCollection, RevenueCollectionManipulationDto>()
			.GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);
		
		config.ForType<RevenueCollectionDetail, RevenueCollectionDetailDto>()
			.Map(src=>src.RevenueTypeName,dest=>dest.RevenueType.Name)
			.GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);
	}
}

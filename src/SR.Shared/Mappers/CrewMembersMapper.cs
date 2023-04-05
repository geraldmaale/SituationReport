using Mapster;
using SR.Shared.DTOs.Crews;
using SR.Shared.Entities;

namespace SR.Shared.Mappers;
public class CrewMembersMapper: IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<CrewProcessing, CrewProcessingDto>()
			.Map(src=>src.Officer,dest=>dest.Officer.FullName)
			.GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);
		
		config.ForType<Embarkation, EmbarkationDto>()
			.Map(src=>src.CrewNationality,dest=>dest.Nationality.Name)
			.GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);

		config.ForType<DisEmbarkation, DisEmbarkationDto>()
			.Map(src=>src.CrewNationality,dest=>dest.Nationality.Name)
			.GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);
		
		config.ForType<VesselsDocked, VesselsDockedDto>()
			.Map(src=>src.VesselTypeName,dest=>dest.VesselType.Name)
			.GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);
		
		config.ForType<AshorePass, AshorePassDto>()
			.Map(src=>src.AshorePassType,dest=>dest.PassType.Name)
			.GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);
	}
}

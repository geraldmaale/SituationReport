using Mapster;
using SR.Shared.DTOs.User;
using SR.Shared.Identity;

namespace SR.Shared.Mappers;

public class ApplicationMappingRegister: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<ApplicationUser, UserCreationDto>()
            .GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);

        config.ForType<ApplicationUser, UserUpdateDto>()
            .GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);

        config.ForType<ApplicationUser, UserWithToken>()
            .GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);

        config.ForType<ApplicationUser, UserDto>();
    }
}

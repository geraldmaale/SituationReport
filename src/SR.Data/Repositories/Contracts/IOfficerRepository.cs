using GreatIdeas.Repository;
using SR.Shared.DTOs.Officers;
using SR.Shared.Entities;

namespace SR.Data.Repositories.Contracts;
public interface IOfficerRepository: IRepositoryFactory<ApplicationDbContext, Officer, OfficerDto>
{
	ValueTask<OfficerDto> CreateOfficer(OfficerCreationDto model, CancellationToken cancellationToken);
	ValueTask<bool> DeleteOfficer(Guid officerId, CancellationToken cancellationToken);
	ValueTask<OfficerFullDto> GetById(Guid officerId, CancellationToken cancellationToken);
}

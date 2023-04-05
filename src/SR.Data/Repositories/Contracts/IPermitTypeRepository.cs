using GreatIdeas.Repository;
using SR.Shared.DTOs.Permits;
using SR.Shared.Entities;

namespace SR.Data.Repositories.Contracts;
// public interface IPermitTypeRepository: IRepositoryFactory<ApplicationDbContext, PermitType, PermitTypeDto>
public interface IPermitTypeRepository
{
	ValueTask DeleteAsync(int id, CancellationToken cancellationToken);
	ValueTask<IReadOnlyList<PermitTypeDto>> GetAllAsync(CancellationToken cancellationToken);
	ValueTask<PermitTypeDto> GetByIdAsync(int id, CancellationToken cancellationToken);
	ValueTask InsertAsync(PermitTypeManipulationDto permitType, CancellationToken cancellationToken);
	ValueTask UpdateAsync(int id, PermitTypeManipulationDto permitType, CancellationToken cancellationToken);
}

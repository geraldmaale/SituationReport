using GreatIdeas.Repository;
using SR.Shared.DTOs.Passports;
using SR.Shared.Entities;

namespace SR.Data.Repositories.Contracts;
public interface IPassportProcessingRepository: IRepositoryFactory<ApplicationDbContext, PassportProcessing, PassportProcessingDto>
{
	ValueTask<PassportProcessing> GetById(Guid id, CancellationToken cancellationToken);
}

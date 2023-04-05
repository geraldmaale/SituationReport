using GreatIdeas.Repository;
using SR.Shared.DTOs.Crews;
using SR.Shared.Entities;
using SR.Shared.Params;

namespace SR.Data.Repositories.Contracts;

public interface ICrewProcessingRepository: IRepositoryFactory<ApplicationDbContext, CrewProcessing, CrewProcessingDto>
{
	ValueTask<CrewProcessing> GetById(Guid id, CancellationToken cancellationToken);
	ValueTask<List<EmbarkationExportDto>> GetEmbarkationDetails(PagingParameters pagingParams);
	ValueTask<List<DisEmbarkationExportDto>> GetDisEmbarkationDetails(PagingParameters pagingParams);
	ValueTask<List<VesselDockedExportDto>> GetVesselsDockedDetails(PagingParameters pagingParams);
	ValueTask<List<AshorePassExportDto>> GetAshorePassDetails(PagingParameters pagingParams);
	ValueTask<CrewProcessingExportDto> GetCrewProcessingDetails(PagingParameters pagingParams);
	ValueTask DeleteEmbarkationDetail(Guid embarkationId, Guid crewProcessingId, CancellationToken cancellationToken);
	ValueTask DeleteDisEmbarkationDetail(Guid disembarkationId, Guid crewProcessingId, CancellationToken cancellationToken);
	ValueTask DeleteVesselDockedDetail(Guid vesselDockedId, Guid crewProcessingId, CancellationToken cancellationToken);
}
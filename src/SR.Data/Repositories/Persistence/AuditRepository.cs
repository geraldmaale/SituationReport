using GreatIdeas.Repository;
using GreatIdeas.Repository.Paging;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SR.Data.Repositories.Contracts;
using SR.Shared.DTOs;
using SR.Shared.DTOs.Audits;
using SR.Shared.Entities;
using SR.Shared.Params;

namespace SR.Data.Repositories.Persistence;

public class AuditRepository : RepositoryFactory<ApplicationDbContext, Audit, AuditDto>, IAuditRepository
{
	private readonly ExportFileHelper _exportFileHelper;

    public AuditRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory, ExportFileHelper exportFileHelper) 
	    : base(dbContextFactory)
    {
	    _exportFileHelper = exportFileHelper;
    }

    public async Task<PagedList<AuditDto>> GetPagedAuditsAsync(AuditPagingParameters pagingParameters,
        CancellationToken cancellationToken)
    {
        var query = FilterAudits(pagingParameters);

        return await PagedList<AuditDto>.ToPagedListAsync(query.ProjectToType<AuditDto>(),
            pagingParameters.PageIndex, pagingParameters.PageSize, cancellationToken);
    }

    public async Task<MemoryStream> Export(AuditPagingParameters pagingParameters, CancellationToken cancellationToken)
    {
        var query = FilterAudits(pagingParameters);

        var exportable = await query.ProjectToType<AuditExportDto>().ToListAsync(cancellationToken);
        
        var generatedExport = _exportFileHelper
	        .GenerateExcel(exportable, pagingParameters.FileName, "Audit Logs", showTitle: true);

        return generatedExport;
    }

    private IQueryable<Audit> FilterAudits(AuditPagingParameters pagingPagingParameters)
    {
	    var dbset = DbContextFactory.CreateDbContext().Set<Audit>();
        var collections = dbset.AsNoTracking().AsQueryable();

        // Filter
        if (!string.IsNullOrWhiteSpace(pagingPagingParameters.Username))
        {
            var param = pagingPagingParameters.Username.Trim().ToLower();
            collections = collections.Where(m => m.Username.ToLower().Equals(param));
        }

        if (!string.IsNullOrWhiteSpace(pagingPagingParameters.Name))
        {
            var param = pagingPagingParameters.Name.Trim().ToLower();
            collections = collections.Where(m => m.FullName.ToLower().Equals(param));
        }

        if (!string.IsNullOrWhiteSpace(pagingPagingParameters.Action))
        {
            var param = pagingPagingParameters.Action.Trim().ToLower();
            collections = collections.Where(m => m.Action.ToLower().Equals(param));
        }

        if (!string.IsNullOrWhiteSpace(pagingPagingParameters.Entity))
        {
            var param = pagingPagingParameters.Entity.Trim().ToLower();
            collections = collections.Where(m => m.TableName.ToLower().Equals(param));
        }

        if (pagingPagingParameters.StartDate != null && pagingPagingParameters.EndDate != null)
        {
            var startDate = DateTime.SpecifyKind(pagingPagingParameters.StartDate.Value, DateTimeKind.Utc);
            var endDate = DateTime.SpecifyKind(pagingPagingParameters.EndDate.Value, DateTimeKind.Utc);

            collections = collections.Where(m => m.Timestamp >= startDate && m.Timestamp <= endDate);
        }

        // Search
        if (!string.IsNullOrWhiteSpace(pagingPagingParameters.Search))
        {
            var searchQuery = pagingPagingParameters.Search.Trim().ToLower();
            collections = collections.Where(a => a.Username.ToLower().Contains(searchQuery)
                                                 || a.FullName.ToLower().Contains(searchQuery)
                                                 || a.Action.ToLower().Contains(searchQuery)
                                                 || a.AffectedColumns.ToLower().Contains(searchQuery)
                                                 || a.TableName.ToLower().Contains(searchQuery));
        }

        // Sort
        /*if (string.IsNullOrWhiteSpace(pagingPagingParameters.OrderBy))
        {
            collections = collections.OrderByDescending(a => a.Timestamp);
        }
        else
        {
            //get property mapping dictionary
            var propertyMappingDictionary = _mappingService.GetPropertyMapping<AuditDto, Audit>();

            collections = collections.ApplySort(pagingPagingParameters.OrderBy, propertyMappingDictionary);
        }*/
        
        collections = collections.OrderByDescending(o => o.Timestamp);

        return collections;
    }
}
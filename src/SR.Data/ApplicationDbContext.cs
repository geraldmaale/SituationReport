using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SR.Shared.Entities;
using SR.Shared.Identity;

namespace SR.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
	private readonly ICurrentUserService _currentUserService;

	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
		ICurrentUserService currentUserService)
		: base(options)
	{
		_currentUserService = currentUserService;
	}

	public DbSet<Audit> AuditLogs { get; set; } = default!;
	public DbSet<Office> Office { get; set; }
	public DbSet<Officer> Officers { get; set; }
	public DbSet<PermitProcessing> PermitProcessings { get; set; }
	public DbSet<PermitProcessingDetail> PermitProcessingDetails { get; set; }
	public DbSet<PassportProcessing> PassportProcessings { get; set; }
	// public DbSet<CrewMembersUnit> CrewMembersUnit { get; set; }
	public DbSet<CrewProcessing> CrewProcessings { get; set; }
	public DbSet<OperationOffice> OperationOffices { get; set; }
	public DbSet<OperationInspectionDetail> OperationInspectionDetails { get; set; }
	public DbSet<OperationInspection> OperationInspections { get; set; }

	public DbSet<Embarkation> Embarkations { get; set; }
	public DbSet<DisEmbarkation> DisEmbarkations { get; set; }
	public DbSet<AshorePass> AshorePasses { get; set; }
	public DbSet<VesselsDocked> VesselsDocked { get; set; }
	public DbSet<RevenueCollection> RevenueCollections { get; set; }
	public DbSet<RevenueCollectionDetail> RevenueCollectionDetails { get; set; }

	public DbSet<NationalityType> Nationalities { get; set; }
	public DbSet<RevenueType> RevenueTypes { get; set; }
	public DbSet<PermitType> PermitTypes { get; set; }
	public DbSet<OperationType> OperationTypes { get; set; }
	public DbSet<VesselType> VesselTypes { get; set; }
	public DbSet<AshorePassType> AshorePassTypes { get; set; }

	protected override void OnModelCreating(ModelBuilder builder)
	{
		// Crew Members processing
		builder.Entity<CrewProcessing>()
			.HasKey(x => x.Id);
		
		builder.Entity<Embarkation>()
			.HasKey(x => x.EmbarkationId);
		builder.Entity<Embarkation>()
			.HasOne(x => x.CrewProcessing)
			.WithMany(x => x.Embarkations)
			.HasForeignKey(x => x.CrewProcessingId);

		builder.Entity<DisEmbarkation>()
			.HasKey(x => x.DisEmbarkationId);
		builder.Entity<DisEmbarkation>()
			.HasOne(x => x.CrewProcessing)
			.WithMany(x => x.DisEmbarkations)
			.HasForeignKey(x => x.CrewProcessingId);

		builder.Entity<VesselsDocked>()
			.HasKey(x => x.VesselDockedId);
		builder.Entity<VesselsDocked>()
			.HasOne(x => x.CrewProcessing)
			.WithMany(x => x.VesselsDocked)
			.HasForeignKey(x => x.CrewProcessingId);

		builder.Entity<PermitProcessingDetail>()
			.HasKey(x => x.PermitProcessingDetailId);
		builder.Entity<PermitProcessingDetail>()
			.HasOne(x => x.PermitProcessing)
			.WithMany(x => x.PermitProcessingDetails)
			.HasForeignKey(x => x.PermitProcessingId);

		builder.Entity<PassportProcessing>()
			.HasKey(x => x.Id);

		builder.Entity<RevenueCollectionDetail>()
			.HasKey(x => x.RevenueCollectionDetailId);
		builder.Entity<RevenueCollectionDetail>()
			.HasOne(x => x.RevenueCollection)
			.WithMany(x => x.RevenueCollectionDetails)
			.HasForeignKey(x => x.RevenueCollectionId);

		builder.Entity<OperationInspectionDetail>()
			.HasKey(x => x.OperationInspectionDetailId);
		builder.Entity<OperationInspectionDetail>()
			.HasOne(x => x.OperationInspection)
			.WithMany(x => x.OperationInspectionDetails)
			.HasForeignKey(x => x.OperationInspectionId);

		builder.Entity<OperationOffice>()
			.HasKey(x => x.OperationOfficeId);
		
		builder.Entity<Officer>()
			.HasMany(x=>x.CrewProcessings)
			.WithOne(x=>x.Officer)
			.OnDelete(DeleteBehavior.Restrict);
		
		builder.Entity<Officer>()
			.HasMany(x=>x.PermitProcessings)
			.WithOne(x=>x.Officer)
			.OnDelete(DeleteBehavior.Restrict);
		
		builder.Entity<Officer>()
			.HasMany(x=>x.PassportProcessings)
			.WithOne(x=>x.Officer)
			.OnDelete(DeleteBehavior.Restrict);
		
		builder.Entity<Officer>()
			.HasMany(x=>x.RevenueCollections)
			.WithOne(x=>x.Officer)
			.OnDelete(DeleteBehavior.Restrict);
		
		builder.Entity<Officer>()
			.HasMany(x=>x.OperationInspections)
			.WithOne(x=>x.Officer)
			.OnDelete(DeleteBehavior.Restrict);

		base.OnModelCreating(builder);
	}

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
	{
		// PersistChanges();
		OnBeforeSaveChanges();
		return base.SaveChangesAsync(cancellationToken);
	}

	/// <summary>
	/// Entity audit trail
	/// </summary>
	private void OnBeforeSaveChanges()
	{
		ChangeTracker.DetectChanges();
		var auditEntries = new List<AuditEntry>();
		foreach (var entry in ChangeTracker.Entries())
		{
			if (entry.Entity is Audit
				|| entry.Entity is IdentityUserClaim<string>
				|| entry.Entity is IdentityRoleClaim<string>
				|| entry.Entity is IdentityUserRole<string>
				|| entry.State == EntityState.Detached
				|| entry.State == EntityState.Unchanged)
				continue;

			var auditEntry = new AuditEntry(entry);

			auditEntry.TableName = entry.Entity.GetType().Name;
			auditEntry.Username = _currentUserService.GetUsername();
			auditEntry.FullName = _currentUserService.GetFullName();
			auditEntry.IpAddress = _currentUserService.GetUserIpAddress();

			auditEntries.Add(auditEntry);

			foreach (var property in entry.Properties)
			{
				string propertyName = property.Metadata.Name;
				if (property.Metadata.IsPrimaryKey())
				{
					auditEntry.KeyValues[propertyName] = property.CurrentValue;
					continue;
				}

				switch (entry.State)
				{
					case EntityState.Added:
						auditEntry.AuditType = AuditType.Create;
						auditEntry.NewValues[propertyName] = property.CurrentValue;
						break;

					case EntityState.Deleted:
						auditEntry.AuditType = AuditType.Delete;
						auditEntry.OldValues[propertyName] = property.OriginalValue;
						break;

					case EntityState.Modified:
						if (property.IsModified)
						{
							auditEntry.ChangedColumns.Add(propertyName);
							auditEntry.AuditType = AuditType.Update;
							auditEntry.OldValues[propertyName] = property.OriginalValue;
							auditEntry.NewValues[propertyName] = property.CurrentValue;
						}
						break;
				}
			}
		}

		foreach (var auditEntry in auditEntries)
		{
			AuditLogs.Add(auditEntry.ToAudit());
		}
	}
}
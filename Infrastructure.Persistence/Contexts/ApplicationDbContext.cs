using Application.Interfaces;

using Domain.Common;
using Domain.Entities;

using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Contexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDateTimeService dateTime, IAuthenticatedUserService authenticatedUser) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        _dateTime = dateTime;
        _authenticatedUser = authenticatedUser;
    }

    private readonly IAuthenticatedUserService _authenticatedUser;

    private readonly IDateTimeService _dateTime;

    public DbSet<Product> Products { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<IAuditableEntity> entry in ChangeTracker.Entries<IAuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                entry.Entity.Created = _dateTime.NowUtc;
                entry.Entity.CreatedBy = _authenticatedUser.UserId;
                break;

                case EntityState.Modified:
                entry.Entity.LastModified = _dateTime.NowUtc;
                entry.Entity.LastModifiedBy = _authenticatedUser.UserId;
                break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        //All Decimals will have 18,6 Range
        foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableProperty property in builder.Model.GetEntityTypes()
        .SelectMany(t => t.GetProperties())
        .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            property.SetColumnType("decimal(18,6)");
        }
        base.OnModelCreating(builder);
    }
}
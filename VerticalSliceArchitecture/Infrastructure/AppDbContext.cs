using Microsoft.EntityFrameworkCore;
using VerticalSliceArchitecture.Common.Abstractions;
using VerticalSliceArchitecture.Domain.Users;

namespace VerticalSliceArchitecture.Infrastructure;

public class AppDbContext : DbContext, IUnitOfWork
{
    public DbSet<User> Users => Set<User>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public async Task<int> CommitChangesAsync(CancellationToken cancellationToken = default)
    {
        if (!ChangeTracker.HasChanges())
        {
            return 0;
        }

        return await SaveChangesAsync(cancellationToken);
    }
}

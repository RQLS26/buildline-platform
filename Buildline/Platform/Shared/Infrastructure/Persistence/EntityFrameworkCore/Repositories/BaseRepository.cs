using Buildline.Platform.Shared.Domain.Repositories;
using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Base repository for all repositories
/// </summary>
/// <remarks>
///     This class is used to define the basic CRUD operations for all repositories.
///     This class implements the IBaseRepository interface.
/// </remarks>
/// <typeparam name="TEntity">
///     The entity type for the repository
/// </typeparam>
public class BaseRepository<TEntity> : IBaseRepository<TEntity>, ICompanyScopedRepository<TEntity> where TEntity : class
{
    /// <summary>
    ///     Gets the shared application database context used by concrete repositories.
    /// </summary>
    protected readonly AppDbContext Context;

    /// <summary>
    ///     Default constructor for the base repository
    /// </summary>
    protected BaseRepository(AppDbContext context)
    {
        Context = context;
    }

    /// <inheritdoc />
    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Context.Set<TEntity>().AddAsync(entity, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TEntity?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken);
    }

    /// <inheritdoc />
    public void Update(TEntity entity)
    {
        Context.Set<TEntity>().Update(entity);
    }

    /// <inheritdoc />
    public void Remove(TEntity entity)
    {
        Context.Set<TEntity>().Remove(entity);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TEntity>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>().ToListAsync(cancellationToken);
    }
    /// <inheritdoc />
    public async Task<IEnumerable<TEntity>> ListByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>()
            .Where(entity => EF.Property<int>(entity, "CompanyId") == companyId)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TEntity?> FindByIdAndCompanyIdAsync(int id, int companyId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>()
            .FirstOrDefaultAsync(entity =>
                EF.Property<int>(entity, "Id") == id &&
                EF.Property<int>(entity, "CompanyId") == companyId,
                cancellationToken);
    }
}

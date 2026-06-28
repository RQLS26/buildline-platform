namespace Buildline.Platform.Shared.Domain.Repositories;

/// <summary>
///     Repository contract for aggregates whose rows are owned by a company profile.
/// </summary>
/// <typeparam name="TEntity">Company-owned aggregate type handled by the repository.</typeparam>
public interface ICompanyScopedRepository<TEntity>
{
    /// <summary>
    ///     Lists only rows that belong to the supplied company profile.
    /// </summary>
    /// <param name="companyId">Company profile identifier that owns the requested rows.</param>
    /// <param name="cancellationToken">Token used to cancel the database query.</param>
    /// <returns>Company-owned rows, or an empty collection when none exist.</returns>
    Task<IEnumerable<TEntity>> ListByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Finds one row by persistence identity and company ownership.
    /// </summary>
    /// <param name="id">Persistence identifier requested by the API client.</param>
    /// <param name="companyId">Company profile identifier that must own the row.</param>
    /// <param name="cancellationToken">Token used to cancel the database query.</param>
    /// <returns>The owned row when found; otherwise <c>null</c>.</returns>
    Task<TEntity?> FindByIdAndCompanyIdAsync(int id, int companyId, CancellationToken cancellationToken = default);
}

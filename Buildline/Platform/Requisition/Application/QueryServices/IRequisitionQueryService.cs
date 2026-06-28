namespace Buildline.Platform.Requisition.Application.QueryServices;

/// <summary>
///     Application query contract for material requisition read use cases.
/// </summary>
public interface IRequisitionQueryService
{
    /// <summary>
    ///     Retrieves every material requisition visible to logistics workflows.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>A requisition collection, possibly empty.</returns>
    Task<IEnumerable<Domain.Model.Aggregates.Requisition>> ListAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves one requisition by persistence identity.
    /// </summary>
    /// <param name="id">Requisition identifier requested by the API client.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>The requisition aggregate when found; otherwise <c>null</c>.</returns>
    Task<Domain.Model.Aggregates.Requisition?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
    /// <summary>
    ///     Retrieves rows owned by the supplied company profile.
    /// </summary>
    /// <param name="companyId">Company profile identifier that owns the requested rows.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>A company-owned collection, possibly empty.</returns>
    Task<IEnumerable<Domain.Model.Aggregates.Requisition>> ListByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves one row by identifier and company ownership.
    /// </summary>
    /// <param name="id">Persistence identifier requested by the API client.</param>
    /// <param name="companyId">Company profile identifier that must own the row.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>The owned row when found; otherwise <c>null</c>.</returns>
    Task<Domain.Model.Aggregates.Requisition?> FindByIdAndCompanyIdAsync(int id, int companyId, CancellationToken cancellationToken = default);
}

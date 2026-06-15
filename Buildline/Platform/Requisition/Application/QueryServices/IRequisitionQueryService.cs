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
}

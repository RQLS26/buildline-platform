using Buildline.Platform.Inventory.Domain.Model.Aggregates;

namespace Buildline.Platform.Inventory.Application.QueryServices;

/// <summary>Application query contract for inventory read use cases.</summary>
public interface IInventoryItemQueryService
{
    /// <summary>Retrieves every inventory item.</summary>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>An inventory collection, possibly empty.</returns>
    Task<IEnumerable<InventoryItem>> ListAsync(CancellationToken cancellationToken = default);

    /// <summary>Retrieves one inventory item by identifier.</summary>
    /// <param name="id">Inventory item persistence identifier.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>The inventory item when found; otherwise <c>null</c>.</returns>
    Task<InventoryItem?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
    /// <summary>
    ///     Retrieves rows owned by the supplied company profile.
    /// </summary>
    /// <param name="companyId">Company profile identifier that owns the requested rows.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>A company-owned collection, possibly empty.</returns>
    Task<IEnumerable<InventoryItem>> ListByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves one row by identifier and company ownership.
    /// </summary>
    /// <param name="id">Persistence identifier requested by the API client.</param>
    /// <param name="companyId">Company profile identifier that must own the row.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>The owned row when found; otherwise <c>null</c>.</returns>
    Task<InventoryItem?> FindByIdAndCompanyIdAsync(int id, int companyId, CancellationToken cancellationToken = default);
}

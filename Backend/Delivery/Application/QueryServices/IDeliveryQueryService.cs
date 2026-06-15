namespace Buildline.Platform.Delivery.Application.QueryServices;

/// <summary>Application query contract for delivery tracking read use cases.</summary>
public interface IDeliveryQueryService
{
    /// <summary>Retrieves every tracked delivery.</summary>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>A delivery collection, possibly empty.</returns>
    Task<IEnumerable<Domain.Model.Aggregates.Delivery>> ListAsync(CancellationToken cancellationToken = default);

    /// <summary>Retrieves one delivery by identifier.</summary>
    /// <param name="id">Delivery persistence identifier.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>The delivery when found; otherwise <c>null</c>.</returns>
    Task<Domain.Model.Aggregates.Delivery?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
}

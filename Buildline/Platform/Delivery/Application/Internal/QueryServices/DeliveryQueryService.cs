using Buildline.Platform.Delivery.Application.QueryServices;
using Buildline.Platform.Delivery.Domain.Repositories;

namespace Buildline.Platform.Delivery.Application.Internal.QueryServices;

/// <summary>Application query service that coordinates delivery tracking reads.</summary>
/// <param name="deliveryRepository">Repository used to retrieve deliveries.</param>
public class DeliveryQueryService(IDeliveryRepository deliveryRepository) : IDeliveryQueryService
{
    /// <inheritdoc />
    public async Task<IEnumerable<Domain.Model.Aggregates.Delivery>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await deliveryRepository.ListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Domain.Model.Aggregates.Delivery?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await deliveryRepository.FindByIdAsync(id, cancellationToken);
    }
    /// <inheritdoc />
    public async Task<IEnumerable<Domain.Model.Aggregates.Delivery>> ListByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default)
    {
        return await deliveryRepository.ListByCompanyIdAsync(companyId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Domain.Model.Aggregates.Delivery?> FindByIdAndCompanyIdAsync(int id, int companyId, CancellationToken cancellationToken = default)
    {
        return await deliveryRepository.FindByIdAndCompanyIdAsync(id, companyId, cancellationToken);
    }
}

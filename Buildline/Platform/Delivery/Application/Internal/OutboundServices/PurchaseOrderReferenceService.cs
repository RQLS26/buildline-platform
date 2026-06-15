using Buildline.Platform.Delivery.Domain.Model.Commands;
using Buildline.Platform.Procurement.Domain.Repositories;

namespace Buildline.Platform.Delivery.Application.Internal.OutboundServices;

/// <summary>
///     Purchase order reference lookup used by delivery tracking write use cases.
/// </summary>
/// <param name="purchaseOrderRepository">Repository for purchase orders owned by the Procurement context.</param>
/// <remarks>
///     Deliveries reference purchase orders by their business code because that is the identifier shown by
///     the frontend. This service preserves that API contract while keeping the validation outside the Delivery
///     aggregate, where cross-context queries do not belong.
/// </remarks>
public class PurchaseOrderReferenceService(IPurchaseOrderRepository purchaseOrderRepository) : IPurchaseOrderReferenceService
{
    /// <inheritdoc />
    public async Task<bool> PurchaseOrderExistsForAsync(
        CreateDeliveryCommand command,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.PurchaseOrder))
            return false;

        var purchaseOrders = await purchaseOrderRepository.ListAsync(cancellationToken);
        return purchaseOrders.Any(order =>
            string.Equals(order.OrderId, command.PurchaseOrder.Trim(), StringComparison.OrdinalIgnoreCase));
    }
}

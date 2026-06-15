using Buildline.Platform.Delivery.Domain.Model.Commands;

namespace Buildline.Platform.Delivery.Application.Internal.OutboundServices;

/// <summary>
///     Outbound service contract used by Delivery to validate purchase order references.
/// </summary>
/// <remarks>
///     Delivery tracking starts after Procurement emits a purchase order. The Delivery command service uses
///     this abstraction to avoid knowing how purchase orders are stored while still rejecting orphan tracking
///     records that would not be actionable in the frontend.
/// </remarks>
public interface IPurchaseOrderReferenceService
{
    /// <summary>
    ///     Checks whether the purchase order code carried by a delivery command exists in Procurement.
    /// </summary>
    /// <param name="command">Delivery creation command whose purchase order code must be validated.</param>
    /// <param name="cancellationToken">Token used to cancel the lookup when the HTTP request is aborted.</param>
    /// <returns><c>true</c> when Procurement can resolve the purchase order code; otherwise <c>false</c>.</returns>
    Task<bool> PurchaseOrderExistsForAsync(
        CreateDeliveryCommand command,
        CancellationToken cancellationToken = default);
}

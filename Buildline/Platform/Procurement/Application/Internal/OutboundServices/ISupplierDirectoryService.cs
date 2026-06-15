using Buildline.Platform.Procurement.Domain.Model.Commands;

namespace Buildline.Platform.Procurement.Application.Internal.OutboundServices;

/// <summary>
///     Outbound service contract used by Procurement to validate supplier directory references.
/// </summary>
/// <remarks>
///     Purchase orders are created from procurement screens that submit a supplier display name. The Suppliers
///     bounded context owns the directory, so Procurement uses this abstraction instead of querying supplier
///     persistence details directly from the command service.
/// </remarks>
public interface ISupplierDirectoryService
{
    /// <summary>
    ///     Checks whether the supplier requested by a purchase order exists and can receive new orders.
    /// </summary>
    /// <param name="command">Command whose supplier name must match an active supplier.</param>
    /// <param name="cancellationToken">Token used to cancel the lookup when the HTTP request is aborted.</param>
    /// <returns><c>true</c> when the supplier exists and is active; otherwise <c>false</c>.</returns>
    Task<bool> SupplierCanReceiveOrdersForAsync(
        CreatePurchaseOrderCommand command,
        CancellationToken cancellationToken = default);
}

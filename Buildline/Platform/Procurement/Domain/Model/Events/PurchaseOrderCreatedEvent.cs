using Buildline.Platform.Shared.Domain.Model.Events;

namespace Buildline.Platform.Procurement.Domain.Model.Events;

/// <summary>
///     Domain event raised when PurchaseOrderCreatedEvent occurs inside the aggregate boundary.
/// </summary>
public sealed record PurchaseOrderCreatedEvent(int PurchaseOrderId, string OrderId, string SupplierName, decimal TotalAmount) : IEvent;

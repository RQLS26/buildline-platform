using Buildline.Platform.Shared.Domain.Model.Events;

namespace Buildline.Platform.Procurement.Domain.Model.Events;

/// <summary>
///     Domain event raised when PurchaseOrderStatusChangedEvent occurs inside the aggregate boundary.
/// </summary>
public sealed record PurchaseOrderStatusChangedEvent(int PurchaseOrderId, string PreviousStatus, string CurrentStatus) : IEvent;

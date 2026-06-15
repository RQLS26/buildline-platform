using Buildline.Platform.Shared.Domain.Model.Events;

namespace Buildline.Platform.Requisition.Domain.Model.Events;

/// <summary>
///     Domain event raised when RequisitionStatusChangedEvent occurs inside the aggregate boundary.
/// </summary>
public sealed record RequisitionStatusChangedEvent(int RequisitionId, string PreviousStatus, string CurrentStatus) : IEvent;

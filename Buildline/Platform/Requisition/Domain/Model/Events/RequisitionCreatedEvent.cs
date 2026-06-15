using Buildline.Platform.Shared.Domain.Model.Events;

namespace Buildline.Platform.Requisition.Domain.Model.Events;

/// <summary>
///     Domain event raised when RequisitionCreatedEvent occurs inside the aggregate boundary.
/// </summary>
public sealed record RequisitionCreatedEvent(int RequisitionId, string ReqId, string Material, string Project) : IEvent;

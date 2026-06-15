using Buildline.Platform.Shared.Domain.Model.Events;

namespace Buildline.Platform.Delivery.Domain.Model.Events;

/// <summary>
///     Domain event raised when DeliveryStatusChangedEvent occurs inside the aggregate boundary.
/// </summary>
public sealed record DeliveryStatusChangedEvent(int DeliveryId, string TrackingId, string PreviousStatus, string CurrentStatus) : IEvent;

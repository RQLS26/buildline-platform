using Buildline.Platform.Shared.Domain.Model.Events;

namespace Buildline.Platform.Analytics.Domain.Model.Events;

/// <summary>
///     Domain event raised when BudgetStatusChangedEvent occurs inside the aggregate boundary.
/// </summary>
public sealed record BudgetStatusChangedEvent(int BudgetId, string Project, string PreviousStatus, string CurrentStatus) : IEvent;

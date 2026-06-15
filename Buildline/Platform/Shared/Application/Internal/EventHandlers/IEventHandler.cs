using Buildline.Platform.Shared.Domain.Model.Events;
using Cortex.Mediator.Notifications;

namespace Buildline.Platform.Shared.Application.Internal.EventHandlers;

/// <summary>
///     Marker interface for application event handlers.
/// </summary>
/// <typeparam name="TEvent">Domain or integration event handled through Cortex notifications.</typeparam>
/// <remarks>
///     The interface aligns Buildline event handlers with the mediator pipeline used in the course
///     sample while preserving a project-specific type name.
/// </remarks>
public interface IEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : IEvent
{
}

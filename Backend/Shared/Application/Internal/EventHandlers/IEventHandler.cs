using Buildline.Platform.Shared.Domain.Model.Events;
using Cortex.Mediator.Notifications;

namespace Buildline.Platform.Shared.Application.Internal.EventHandlers;

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : IEvent
{
}
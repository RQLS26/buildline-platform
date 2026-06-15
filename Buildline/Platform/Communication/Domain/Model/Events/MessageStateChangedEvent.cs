using Buildline.Platform.Shared.Domain.Model.Events;

namespace Buildline.Platform.Communication.Domain.Model.Events;

/// <summary>
///     Domain event raised when MessageStateChangedEvent occurs inside the aggregate boundary.
/// </summary>
public sealed record MessageStateChangedEvent(int MessageId, bool PreviousReadState, bool CurrentReadState, bool PreviousStarredState, bool CurrentStarredState) : IEvent;

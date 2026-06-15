using Buildline.Platform.Shared.Domain.Model.Events;

namespace Buildline.Platform.Shared.Domain.Model.Entities;

/// <summary>
///     Exposes domain events raised by an aggregate during command handling.
/// </summary>
/// <remarks>
///     The current Web Services delivery records events inside aggregates so application services can
///     publish them later through the shared mediator pipeline without changing domain behavior.
/// </remarks>
public interface IHasDomainEvents
{
    /// <summary>Gets events raised by the aggregate but not yet dispatched.</summary>
    IReadOnlyCollection<IEvent> DomainEvents { get; }

    /// <summary>Clears the pending domain events after dispatch or persistence.</summary>
    void ClearDomainEvents();
}

using DeliveryAggregate = Buildline.Platform.Delivery.Domain.Model.Aggregates.Delivery;
using Buildline.Platform.Delivery.Domain.Model.Commands;
using Buildline.Platform.Shared.Application.Model;

namespace Buildline.Platform.Delivery.Application.CommandServices;

/// <summary>
///     Defines application write use cases for delivery aggregates.
/// </summary>
public interface IDeliveryCommandService
{
    /// <summary>
    ///     Handles creation of a new delivery aggregate.
    /// </summary>
    /// <param name="command">Command containing the values accepted by the REST contract.</param>
    /// <param name="cancellationToken">Token used to cancel repository and unit-of-work operations.</param>
    /// <returns>A successful result with the created aggregate, or a typed bounded-context error.</returns>
    Task<Result<DeliveryAggregate>> Handle(CreateDeliveryCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handles partial update of an existing delivery aggregate.
    /// </summary>
    /// <param name="command">Command containing the aggregate id and replacement values.</param>
    /// <param name="cancellationToken">Token used to cancel lookup, mutation and persistence.</param>
    /// <returns>A successful result with the updated aggregate, or a typed bounded-context error.</returns>
    Task<Result<DeliveryAggregate>> Handle(UpdateDeliveryCommand command, CancellationToken cancellationToken = default);
}


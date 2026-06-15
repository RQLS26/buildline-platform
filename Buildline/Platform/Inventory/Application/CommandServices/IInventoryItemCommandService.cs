using Buildline.Platform.Inventory.Domain.Model.Aggregates;
using Buildline.Platform.Inventory.Domain.Model.Commands;
using Buildline.Platform.Shared.Application.Model;

namespace Buildline.Platform.Inventory.Application.CommandServices;

/// <summary>
///     Defines application write use cases for inventory item aggregates.
/// </summary>
public interface IInventoryItemCommandService
{
    /// <summary>
    ///     Handles creation of a new inventory item aggregate.
    /// </summary>
    /// <param name="command">Command containing the values accepted by the REST contract.</param>
    /// <param name="cancellationToken">Token used to cancel repository and unit-of-work operations.</param>
    /// <returns>A successful result with the created aggregate, or a typed bounded-context error.</returns>
    Task<Result<InventoryItem>> Handle(CreateInventoryItemCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handles partial update of an existing inventory item aggregate.
    /// </summary>
    /// <param name="command">Command containing the aggregate id and replacement values.</param>
    /// <param name="cancellationToken">Token used to cancel lookup, mutation and persistence.</param>
    /// <returns>A successful result with the updated aggregate, or a typed bounded-context error.</returns>
    Task<Result<InventoryItem>> Handle(UpdateInventoryItemCommand command, CancellationToken cancellationToken = default);
}

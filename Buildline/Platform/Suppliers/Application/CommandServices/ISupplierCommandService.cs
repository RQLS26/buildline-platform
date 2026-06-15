using Buildline.Platform.Suppliers.Domain.Model.Aggregates;
using Buildline.Platform.Suppliers.Domain.Model.Commands;
using Buildline.Platform.Shared.Application.Model;

namespace Buildline.Platform.Suppliers.Application.CommandServices;

/// <summary>
///     Defines application write use cases for supplier aggregates.
/// </summary>
public interface ISupplierCommandService
{
    /// <summary>
    ///     Handles creation of a new supplier aggregate.
    /// </summary>
    /// <param name="command">Command containing the values accepted by the REST contract.</param>
    /// <param name="cancellationToken">Token used to cancel repository and unit-of-work operations.</param>
    /// <returns>A successful result with the created aggregate, or a typed bounded-context error.</returns>
    Task<Result<Supplier>> Handle(CreateSupplierCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handles partial update of an existing supplier aggregate.
    /// </summary>
    /// <param name="command">Command containing the aggregate id and replacement values.</param>
    /// <param name="cancellationToken">Token used to cancel lookup, mutation and persistence.</param>
    /// <returns>A successful result with the updated aggregate, or a typed bounded-context error.</returns>
    Task<Result<Supplier>> Handle(UpdateSupplierCommand command, CancellationToken cancellationToken = default);
    /// <summary>
    ///     Handles removal of an existing supplier aggregate.
    /// </summary>
    /// <param name="supplierId">Identifier of the aggregate to remove.</param>
    /// <param name="cancellationToken">Token used to cancel lookup and persistence.</param>
    /// <returns>A successful empty result, or a typed bounded-context error.</returns>
    Task<Result> HandleDelete(int supplierId, CancellationToken cancellationToken = default);
}


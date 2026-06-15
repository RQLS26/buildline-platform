using RequisitionAggregate = Buildline.Platform.Requisition.Domain.Model.Aggregates.Requisition;
using Buildline.Platform.Requisition.Domain.Model.Commands;
using Buildline.Platform.Shared.Application.Model;

namespace Buildline.Platform.Requisition.Application.CommandServices;

/// <summary>
///     Defines application write use cases for requisition aggregates.
/// </summary>
public interface IRequisitionCommandService
{
    /// <summary>
    ///     Handles creation of a new requisition aggregate.
    /// </summary>
    /// <param name="command">Command containing the values accepted by the REST contract.</param>
    /// <param name="cancellationToken">Token used to cancel repository and unit-of-work operations.</param>
    /// <returns>A successful result with the created aggregate, or a typed bounded-context error.</returns>
    Task<Result<RequisitionAggregate>> Handle(CreateRequisitionCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handles partial update of an existing requisition aggregate.
    /// </summary>
    /// <param name="command">Command containing the aggregate id and replacement values.</param>
    /// <param name="cancellationToken">Token used to cancel lookup, mutation and persistence.</param>
    /// <returns>A successful result with the updated aggregate, or a typed bounded-context error.</returns>
    Task<Result<RequisitionAggregate>> Handle(UpdateRequisitionCommand command, CancellationToken cancellationToken = default);
}


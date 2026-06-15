using Buildline.Platform.Analytics.Domain.Model.Aggregates;
using Buildline.Platform.Analytics.Domain.Model.Commands;
using Buildline.Platform.Shared.Application.Model;

namespace Buildline.Platform.Analytics.Application.CommandServices;

/// <summary>
///     Defines application write use cases for budget aggregates.
/// </summary>
public interface IBudgetCommandService
{
    /// <summary>
    ///     Handles creation of a new budget aggregate.
    /// </summary>
    /// <param name="command">Command containing the values accepted by the REST contract.</param>
    /// <param name="cancellationToken">Token used to cancel repository and unit-of-work operations.</param>
    /// <returns>A successful result with the created aggregate, or a typed bounded-context error.</returns>
    Task<Result<Budget>> Handle(CreateBudgetCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handles partial update of an existing budget aggregate.
    /// </summary>
    /// <param name="command">Command containing the aggregate id and replacement values.</param>
    /// <param name="cancellationToken">Token used to cancel lookup, mutation and persistence.</param>
    /// <returns>A successful result with the updated aggregate, or a typed bounded-context error.</returns>
    Task<Result<Budget>> Handle(UpdateBudgetCommand command, CancellationToken cancellationToken = default);
}

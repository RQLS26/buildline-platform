using Buildline.Platform.Analytics.Domain.Model.Commands;
using Buildline.Platform.Analytics.Interfaces.Rest.Resources;

namespace Buildline.Platform.Analytics.Interfaces.Rest.Transform;

/// <summary>
///     Converts REST write resources into update commands for the application layer.
/// </summary>
public static class UpdateBudgetCommandFromResourceAssembler
{
    /// <summary>
    ///     Builds an update command from the route identifier and HTTP request body.
    /// </summary>
    /// <param name="budgetId">Aggregate identifier from the route.</param>
    /// <param name="resource">Resource received by the REST endpoint.</param>
    /// <returns>A command containing the target aggregate id and nullable replacement values.</returns>
    public static UpdateBudgetCommand ToCommandFromResource(int budgetId, BudgetWriteResource resource)
    {
        return new UpdateBudgetCommand(
            budgetId,
            resource.Project,
            resource.TotalBudget,
            resource.Spent,
            resource.Allocated,
            resource.Status);
    }
}

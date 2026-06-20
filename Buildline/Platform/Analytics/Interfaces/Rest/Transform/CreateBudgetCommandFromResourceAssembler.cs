using Buildline.Platform.Analytics.Domain.Model.Commands;
using Buildline.Platform.Analytics.Interfaces.Rest.Resources;

namespace Buildline.Platform.Analytics.Interfaces.Rest.Transform;

/// <summary>
///     Converts REST write resources into create commands for the application layer.
/// </summary>
public static class CreateBudgetCommandFromResourceAssembler
{
    /// <summary>
    ///     Builds a create command from the HTTP request body.
    /// </summary>
    /// <param name="companyId">Company profile identifier resolved from the company-scoped route.</param>
    /// <param name="resource">Resource received by the REST endpoint.</param>
    /// <returns>A command containing the same write values without exposing REST types to the domain.</returns>
    public static CreateBudgetCommand ToCommandFromResource(BudgetWriteResource resource, int companyId = 1)
    {
        return new CreateBudgetCommand(
            resource.Project,
            resource.TotalBudget,
            resource.Spent,
            resource.Allocated,
            resource.Status,
            companyId);
    }
}

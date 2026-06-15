using Buildline.Platform.Analytics.Domain.Model.Aggregates;
using Buildline.Platform.Analytics.Interfaces.Rest.Resources;

namespace Buildline.Platform.Analytics.Interfaces.Rest.Transform;

/// <summary>Assembler that maps budget aggregates to REST resources.</summary>
public static class BudgetResourceFromEntityAssembler
{
    /// <summary>Converts a budget aggregate into the frontend budget resource contract.</summary>
    /// <param name="budget">Budget aggregate retrieved from persistence.</param>
    /// <returns>Budget REST resource.</returns>
    public static BudgetResource ToResourceFromEntity(Budget budget)
    {
        return new BudgetResource(budget.Id, budget.Project, budget.TotalBudget, budget.Spent, budget.Allocated, budget.Status);
    }
}

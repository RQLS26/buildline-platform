using Buildline.Platform.Analytics.Domain.Model.Commands;
using Buildline.Platform.Shared.Domain.Model.Entities;

namespace Buildline.Platform.Analytics.Domain.Model.Aggregates;

/// <summary>
///     Aggregate root that represents the budget state of a construction project.
/// </summary>
/// <remarks>
///     Analytics and Budgeting consumes procurement and project data to expose management KPIs. This
///     first Sprint 3 aggregate keeps the frontend dashboard contract stable while giving the bounded
///     context its own persistence and API boundary.
/// </remarks>
public partial class Budget : IAuditableEntity
{
    /// <summary>Initializes an empty budget for Entity Framework Core materialization.</summary>
    protected Budget()
    {
        Project = string.Empty;
        Status = string.Empty;
    }

    /// <summary>Creates a budget aggregate from the frontend budgeting contract.</summary>
    /// <param name="command">Budget payload submitted by management or seed import workflows.</param>
    public Budget(CreateBudgetCommand command)
    {
        Project = command.Project?.Trim() ?? string.Empty;
        TotalBudget = command.TotalBudget ?? 0m;
        Spent = command.Spent ?? 0m;
        Allocated = command.Allocated ?? 0m;
        Status = command.Status?.Trim() ?? "On Track";
    }

    /// <summary>Gets the database-generated budget identifier.</summary>
    public int Id { get; private set; }

    /// <summary>Gets the project represented by this budget row.</summary>
    public string Project { get; private set; }

    /// <summary>Gets the total budget approved for the project.</summary>
    public decimal TotalBudget { get; private set; }

    /// <summary>Gets the actual amount spent.</summary>
    public decimal Spent { get; private set; }

    /// <summary>Gets the amount allocated to near-term procurement commitments.</summary>
    public decimal Allocated { get; private set; }

    /// <summary>Gets the calculated budget status displayed by dashboards.</summary>
    public string Status { get; private set; }

    /// <summary>Gets or sets the audit timestamp captured when the budget row is created.</summary>
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>Gets or sets the audit timestamp captured when the budget row is updated.</summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>Applies a partial budget update.</summary>
    /// <param name="command">Budget fields to replace.</param>
    public void Apply(UpdateBudgetCommand command)
    {
        Project = command.Project is null ? Project : command.Project.Trim();
        TotalBudget = command.TotalBudget ?? TotalBudget;
        Spent = command.Spent ?? Spent;
        Allocated = command.Allocated ?? Allocated;
        Status = command.Status is null ? Status : command.Status.Trim();
    }
}




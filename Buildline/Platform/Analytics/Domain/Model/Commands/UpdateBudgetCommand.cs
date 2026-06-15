namespace Buildline.Platform.Analytics.Domain.Model.Commands;

/// <summary>
///     Command that requests a partial update for an existing budget aggregate.
/// </summary>
/// <param name="BudgetId">Persistence identifier of the aggregate selected by the route.</param>
/// <param name="Project">Write value for the Project field in the frontend-compatible contract.</param>
/// <param name="TotalBudget">Write value for the TotalBudget field in the frontend-compatible contract.</param>
/// <param name="Spent">Write value for the Spent field in the frontend-compatible contract.</param>
/// <param name="Allocated">Write value for the Allocated field in the frontend-compatible contract.</param>
/// <param name="Status">Write value for the Status field in the frontend-compatible contract.</param>
/// <remarks>
///     Nullable fields preserve PATCH semantics: omitted properties keep the current aggregate value.
/// </remarks>
public record UpdateBudgetCommand(
    int BudgetId,
    string? Project,
    decimal? TotalBudget,
    decimal? Spent,
    decimal? Allocated,
    string? Status);

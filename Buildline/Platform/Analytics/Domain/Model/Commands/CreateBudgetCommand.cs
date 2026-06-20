namespace Buildline.Platform.Analytics.Domain.Model.Commands;

/// <summary>
///     Command that requests creation of a budget aggregate from a REST write payload.
/// </summary>
/// <param name="Project">Write value for the Project field in the frontend-compatible contract.</param>
/// <param name="TotalBudget">Write value for the TotalBudget field in the frontend-compatible contract.</param>
/// <param name="Spent">Write value for the Spent field in the frontend-compatible contract.</param>
/// <param name="Allocated">Write value for the Allocated field in the frontend-compatible contract.</param>
/// <param name="Status">Write value for the Status field in the frontend-compatible contract.</param>
/// <param name="CompanyId">Company profile identifier that owns the created operational record.</param>
/// <remarks>
///     The command keeps HTTP resources outside the domain model and lets the application service own
///     validation, persistence coordination and error translation.
/// </remarks>
public record CreateBudgetCommand(
    string? Project,
    decimal? TotalBudget,
    decimal? Spent,
    decimal? Allocated,
    string? Status,
    int CompanyId = 1);

namespace Buildline.Platform.Projects.Domain.Model.Queries;

/// <summary>
///     Query that requests every construction project available as shared reference data.
/// </summary>
/// <remarks>
///     This query supports US-004 filters and the dashboard/budgeting screens that need a complete
///     project list without mutating project state.
/// </remarks>
public record GetAllProjectsQuery;

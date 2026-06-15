namespace Buildline.Platform.Projects.Interfaces.Rest.Resources;

/// <summary>
///     REST resource returned by project endpoints.
/// </summary>
/// <param name="Id">Project identifier used by frontend filters and related records.</param>
/// <param name="Name">Project name displayed in UI lists.</param>
/// <param name="Location">Project site location.</param>
/// <param name="Budget">Approved project budget amount.</param>
/// <param name="Spent">Current spent amount associated with the project.</param>
/// <param name="Date">Frontend-compatible project date string.</param>
/// <param name="Status">Current project status label.</param>
/// <param name="Progress">Completion percentage shown by dashboard widgets.</param>
public record ProjectResource(
    int Id,
    string Name,
    string Location,
    decimal Budget,
    decimal Spent,
    string Date,
    string Status,
    int Progress);

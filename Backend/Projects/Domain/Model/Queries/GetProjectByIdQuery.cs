namespace Buildline.Platform.Projects.Domain.Model.Queries;

/// <summary>
///     Query that requests one construction project by its database identifier.
/// </summary>
/// <param name="ProjectId">Identifier of the project requested by a frontend view or another context.</param>
public record GetProjectByIdQuery(int ProjectId);

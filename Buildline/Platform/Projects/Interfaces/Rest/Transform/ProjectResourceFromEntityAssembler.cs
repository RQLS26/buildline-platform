using Buildline.Platform.Projects.Domain.Model.Aggregates;
using Buildline.Platform.Projects.Interfaces.Rest.Resources;

namespace Buildline.Platform.Projects.Interfaces.Rest.Transform;

/// <summary>
///     Assembler that converts project aggregates into REST resources.
/// </summary>
/// <remarks>
///     The assembler keeps controllers from exposing domain aggregates directly and preserves a stable
///     contract for the frontend while the internal domain model evolves.
/// </remarks>
public static class ProjectResourceFromEntityAssembler
{
    /// <summary>
    ///     Converts a project aggregate to the resource returned by the Projects API.
    /// </summary>
    /// <param name="project">Project aggregate retrieved from persistence.</param>
    /// <returns>A frontend-compatible project resource.</returns>
    public static ProjectResource ToResourceFromEntity(Project project)
    {
        return new ProjectResource(
            project.Id,
            project.Name,
            project.Location,
            project.Budget,
            project.Spent,
            project.Date,
            project.Status,
            project.Progress);
    }
}

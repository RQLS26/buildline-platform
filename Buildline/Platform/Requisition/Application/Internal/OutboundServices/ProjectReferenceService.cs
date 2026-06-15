using Buildline.Platform.Analytics.Interfaces.Acl;
using Buildline.Platform.Requisition.Domain.Model.Commands;

namespace Buildline.Platform.Requisition.Application.Internal.OutboundServices;

/// <summary>
///     ACL-backed project reference validator for requisition write use cases.
/// </summary>
/// <param name="projectsContextFacade">Analytics anti-corruption facade that resolves project reference data.</param>
/// <remarks>
///     The service exists because Requisition stores the project display value used by the current frontend,
///     while Analytics owns the project reference collection. Keeping the dependency behind an outbound
///     service avoids direct coupling from Requisition command services to Analytics persistence classes.
/// </remarks>
public class ProjectReferenceService(IProjectsContextFacade projectsContextFacade) : IProjectReferenceService
{
    /// <inheritdoc />
    public async Task<bool> ProjectExistsForAsync(
        CreateRequisitionCommand command,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Project))
            return false;

        var projectId = await projectsContextFacade.FetchProjectIdByName(command.Project.Trim(), cancellationToken);
        return projectId > 0;
    }
}

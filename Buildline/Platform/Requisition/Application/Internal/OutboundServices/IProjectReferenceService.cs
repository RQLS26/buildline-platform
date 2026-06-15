using Buildline.Platform.Requisition.Domain.Model.Commands;

namespace Buildline.Platform.Requisition.Application.Internal.OutboundServices;

/// <summary>
///     Outbound service contract used by the Requisition application layer to verify project references.
/// </summary>
/// <remarks>
///     Requisition commands receive the project as a frontend-facing display value. This service hides the
///     Analytics ACL lookup so the Requisition command service can validate business references without
///     depending on the Analytics aggregate, repository or EF Core configuration.
/// </remarks>
public interface IProjectReferenceService
{
    /// <summary>
    ///     Checks whether the project submitted in a create requisition command exists in Analytics.
    /// </summary>
    /// <param name="command">Command whose <see cref="CreateRequisitionCommand.Project" /> value must be validated.</param>
    /// <param name="cancellationToken">Token used to cancel the lookup when the HTTP request is aborted.</param>
    /// <returns><c>true</c> when Analytics can resolve the project name; otherwise <c>false</c>.</returns>
    Task<bool> ProjectExistsForAsync(CreateRequisitionCommand command, CancellationToken cancellationToken = default);
}

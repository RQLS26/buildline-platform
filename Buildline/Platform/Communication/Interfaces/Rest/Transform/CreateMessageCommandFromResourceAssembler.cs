using Buildline.Platform.Communication.Domain.Model.Commands;
using Buildline.Platform.Communication.Interfaces.Rest.Resources;

namespace Buildline.Platform.Communication.Interfaces.Rest.Transform;

/// <summary>
///     Converts REST write resources into create commands for the application layer.
/// </summary>
public static class CreateMessageCommandFromResourceAssembler
{
    /// <summary>
    ///     Builds a create command from the HTTP request body.
    /// </summary>
    /// <param name="companyId">Company profile identifier resolved from the company-scoped route.</param>
    /// <param name="resource">Resource received by the REST endpoint.</param>
    /// <returns>A command containing the same write values without exposing REST types to the domain.</returns>
    public static CreateMessageCommand ToCommandFromResource(MessageWriteResource resource, int companyId = 1)
    {
        return new CreateMessageCommand(
            resource.Sender,
            resource.Subject,
            resource.Preview,
            resource.Icon,
            resource.IconClass,
            resource.Label,
            resource.LabelClass,
            resource.IsRead,
            resource.Starred,
            resource.Category,
            resource.Time,
            resource.Date,
            companyId);
    }
}

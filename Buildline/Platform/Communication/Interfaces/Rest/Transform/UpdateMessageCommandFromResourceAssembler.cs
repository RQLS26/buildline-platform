using Buildline.Platform.Communication.Domain.Model.Commands;
using Buildline.Platform.Communication.Interfaces.Rest.Resources;

namespace Buildline.Platform.Communication.Interfaces.Rest.Transform;

/// <summary>
///     Converts REST write resources into update commands for the application layer.
/// </summary>
public static class UpdateMessageCommandFromResourceAssembler
{
    /// <summary>
    ///     Builds an update command from the route identifier and HTTP request body.
    /// </summary>
    /// <param name="messageId">Aggregate identifier from the route.</param>
    /// <param name="resource">Resource received by the REST endpoint.</param>
    /// <returns>A command containing the target aggregate id and nullable replacement values.</returns>
    public static UpdateMessageCommand ToCommandFromResource(int messageId, MessageWriteResource resource)
    {
        return new UpdateMessageCommand(
            messageId,
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
            resource.Date);
    }
}

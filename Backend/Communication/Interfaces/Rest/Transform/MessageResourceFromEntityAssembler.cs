using Buildline.Platform.Communication.Domain.Model.Aggregates;
using Buildline.Platform.Communication.Interfaces.Rest.Resources;

namespace Buildline.Platform.Communication.Interfaces.Rest.Transform;

/// <summary>Assembler that maps message aggregates to REST resources.</summary>
public static class MessageResourceFromEntityAssembler
{
    /// <summary>Converts a message aggregate into the frontend inbox resource contract.</summary>
    /// <param name="message">Message aggregate retrieved from persistence.</param>
    /// <returns>Message REST resource.</returns>
    public static MessageResource ToResourceFromEntity(Message message)
    {
        return new MessageResource(
            message.Id,
            message.Sender,
            message.Subject,
            message.Preview,
            message.Icon,
            message.IconClass,
            message.Label,
            message.LabelClass,
            message.IsRead,
            message.Starred,
            message.Category,
            message.Time,
            message.Date);
    }
}

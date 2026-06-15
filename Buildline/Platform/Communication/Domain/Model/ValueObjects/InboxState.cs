namespace Buildline.Platform.Communication.Domain.Model.ValueObjects;

/// <summary>Value object that groups user-facing inbox state flags.</summary>
/// <param name="IsRead">Indicates whether the message was read.</param>
/// <param name="Starred">Indicates whether the message is marked as important.</param>
public readonly record struct InboxState(bool IsRead, bool Starred)
{
    /// <summary>Creates an inbox state from nullable write fields.</summary>
    public static InboxState From(bool? isRead, bool? starred)
    {
        return new InboxState(isRead ?? false, starred ?? false);
    }
}

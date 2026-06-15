using Buildline.Platform.Communication.Domain.Model.Commands;
using Buildline.Platform.Shared.Domain.Model.Entities;

namespace Buildline.Platform.Communication.Domain.Model.Aggregates;

/// <summary>
///     Aggregate root that represents an internal notification or message.
/// </summary>
/// <remarks>
///     Communication owns inbox state such as read/unread, starred messages, category labels and alert
///     previews. Operational contexts can later publish domain events that create these records.
/// </remarks>
public partial class Message : IAuditableEntity
{
    /// <summary>Initializes an empty message for Entity Framework Core materialization.</summary>
    protected Message()
    {
        Sender = string.Empty;
        Subject = string.Empty;
        Preview = string.Empty;
        Icon = string.Empty;
        IconClass = string.Empty;
        Label = string.Empty;
        LabelClass = string.Empty;
        Category = string.Empty;
        Time = string.Empty;
        Date = string.Empty;
    }

    /// <summary>Creates a message aggregate from the communication application command contract.</summary>
    /// <param name="command">Message payload submitted by an internal notification workflow.</param>
    public Message(CreateMessageCommand command)
    {
        Sender = command.Sender?.Trim() ?? "System";
        Subject = command.Subject?.Trim() ?? string.Empty;
        Preview = command.Preview?.Trim() ?? string.Empty;
        Icon = command.Icon?.Trim() ?? "pi-inbox";
        IconClass = command.IconClass?.Trim() ?? "icon-neutral";
        Label = command.Label?.Trim() ?? string.Empty;
        LabelClass = command.LabelClass?.Trim() ?? string.Empty;
        IsRead = command.IsRead ?? false;
        Starred = command.Starred ?? false;
        Category = command.Category?.Trim() ?? "updates";
        Time = command.Time?.Trim() ?? "now";
        Date = command.Date?.Trim() ?? DateTime.UtcNow.ToString("yyyy-MM-dd");
    }

    /// <summary>Gets the database-generated message identifier.</summary>
    public int Id { get; private set; }

    /// <summary>Gets the sender display name.</summary>
    public string Sender { get; private set; }

    /// <summary>Gets the message subject.</summary>
    public string Subject { get; private set; }

    /// <summary>Gets the short preview shown by the inbox list.</summary>
    public string Preview { get; private set; }

    /// <summary>Gets the icon token used by the frontend.</summary>
    public string Icon { get; private set; }

    /// <summary>Gets the icon CSS class used by the frontend.</summary>
    public string IconClass { get; private set; }

    /// <summary>Gets the optional label text.</summary>
    public string Label { get; private set; }

    /// <summary>Gets the optional label CSS class.</summary>
    public string LabelClass { get; private set; }

    /// <summary>Gets whether the user has read the message.</summary>
    public bool IsRead { get; private set; }

    /// <summary>Gets whether the user starred the message.</summary>
    public bool Starred { get; private set; }

    /// <summary>Gets the message category, such as alerts or updates.</summary>
    public string Category { get; private set; }

    /// <summary>Gets the relative time display value.</summary>
    public string Time { get; private set; }

    /// <summary>Gets the message date.</summary>
    public string Date { get; private set; }

    /// <summary>Gets or sets the audit timestamp captured when the message is created.</summary>
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>Gets or sets the audit timestamp captured when the message is updated.</summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>Applies a partial inbox update such as mark-as-read or toggle-star.</summary>
    /// <param name="command">Message fields to replace.</param>
    public void Apply(UpdateMessageCommand command)
    {
        Sender = command.Sender is null ? Sender : command.Sender.Trim();
        Subject = command.Subject is null ? Subject : command.Subject.Trim();
        Preview = command.Preview is null ? Preview : command.Preview.Trim();
        Icon = command.Icon is null ? Icon : command.Icon.Trim();
        IconClass = command.IconClass is null ? IconClass : command.IconClass.Trim();
        Label = command.Label is null ? Label : command.Label.Trim();
        LabelClass = command.LabelClass is null ? LabelClass : command.LabelClass.Trim();
        IsRead = command.IsRead ?? IsRead;
        Starred = command.Starred ?? Starred;
        Category = command.Category is null ? Category : command.Category.Trim();
        Time = command.Time is null ? Time : command.Time.Trim();
        Date = command.Date is null ? Date : command.Date.Trim();
    }
}




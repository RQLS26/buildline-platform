using Buildline.Platform.Communication.Interfaces.Rest.Resources;
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

    /// <summary>Creates a message aggregate from the communication frontend contract.</summary>
    /// <param name="resource">Message payload submitted by an internal notification workflow.</param>
    public Message(MessageWriteResource resource)
    {
        Sender = resource.Sender?.Trim() ?? "System";
        Subject = resource.Subject?.Trim() ?? string.Empty;
        Preview = resource.Preview?.Trim() ?? string.Empty;
        Icon = resource.Icon?.Trim() ?? "pi-inbox";
        IconClass = resource.IconClass?.Trim() ?? "icon-neutral";
        Label = resource.Label?.Trim() ?? string.Empty;
        LabelClass = resource.LabelClass?.Trim() ?? string.Empty;
        IsRead = resource.IsRead ?? false;
        Starred = resource.Starred ?? false;
        Category = resource.Category?.Trim() ?? "updates";
        Time = resource.Time?.Trim() ?? "now";
        Date = resource.Date?.Trim() ?? DateTime.UtcNow.ToString("yyyy-MM-dd");
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
    /// <param name="resource">Message fields to replace.</param>
    public void Apply(MessageWriteResource resource)
    {
        Sender = resource.Sender is null ? Sender : resource.Sender.Trim();
        Subject = resource.Subject is null ? Subject : resource.Subject.Trim();
        Preview = resource.Preview is null ? Preview : resource.Preview.Trim();
        Icon = resource.Icon is null ? Icon : resource.Icon.Trim();
        IconClass = resource.IconClass is null ? IconClass : resource.IconClass.Trim();
        Label = resource.Label is null ? Label : resource.Label.Trim();
        LabelClass = resource.LabelClass is null ? LabelClass : resource.LabelClass.Trim();
        IsRead = resource.IsRead ?? IsRead;
        Starred = resource.Starred ?? Starred;
        Category = resource.Category is null ? Category : resource.Category.Trim();
        Time = resource.Time is null ? Time : resource.Time.Trim();
        Date = resource.Date is null ? Date : resource.Date.Trim();
    }
}

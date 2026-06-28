using Buildline.Platform.Shared.Domain.Model.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using Buildline.Platform.Communication.Domain.Model.Commands;
using Buildline.Platform.Communication.Domain.Model.Events;
using Buildline.Platform.Communication.Domain.Model.ValueObjects;
using Buildline.Platform.Shared.Domain.Model.Events;

namespace Buildline.Platform.Communication.Domain.Model.Aggregates;

/// <summary>
///     Aggregate root that represents an internal notification or message.
/// </summary>
public partial class Message : IHasDomainEvents
{
    private readonly List<IEvent> _domainEvents = [];

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

    /// <summary>Creates a message aggregate from a communication command.</summary>
    /// <param name="command">Command carrying message values accepted by the application layer.</param>
    public Message(CreateMessageCommand command)
    {
        CompanyId = command.CompanyId;
        var inboxState = InboxState.From(command.IsRead, command.Starred);
        Sender = command.Sender?.Trim() ?? "System";
        Subject = command.Subject?.Trim() ?? string.Empty;
        Preview = command.Preview?.Trim() ?? string.Empty;
        Icon = command.Icon?.Trim() ?? "pi-inbox";
        IconClass = command.IconClass?.Trim() ?? "icon-neutral";
        Label = command.Label?.Trim() ?? string.Empty;
        LabelClass = command.LabelClass?.Trim() ?? string.Empty;
        IsRead = inboxState.IsRead;
        Starred = inboxState.Starred;
        Category = command.Category?.Trim() ?? "updates";
        Time = command.Time?.Trim() ?? "now";
        Date = command.Date?.Trim() ?? DateTime.UtcNow.ToString("yyyy-MM-dd");
    }

    /// <summary>Gets the database-generated message identifier.</summary>
    public int Id { get; private set; }

    /// <summary>Gets the company profile identifier that owns this operational record.</summary>
    public int CompanyId { get; private set; }

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

    /// <inheritdoc />
    [NotMapped]
    public IReadOnlyCollection<IEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <inheritdoc />
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    /// <summary>Applies a partial inbox update such as mark-as-read or toggle-star.</summary>
    /// <param name="command">Command containing replacement values.</param>
    public void Apply(UpdateMessageCommand command)
    {
        var previousReadState = IsRead;
        var previousStarredState = Starred;
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

        if (previousReadState != IsRead || previousStarredState != Starred)
            AddDomainEvent(new MessageStateChangedEvent(Id, previousReadState, IsRead, previousStarredState, Starred));
    }

    /// <summary>Records a domain event raised by this aggregate.</summary>
    /// <param name="domainEvent">Event that describes a completed domain change.</param>
    private void AddDomainEvent(IEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}

using Buildline.Platform.Shared.Domain.Model.Entities;

namespace Buildline.Platform.Communication.Domain.Model.Aggregates;

/// <summary>
///     Audit timestamp projection for the <see cref="Message" /> aggregate.
/// </summary>
public partial class Message : IAuditableEntity
{
    /// <summary>
    ///     Gets or sets the timestamp captured when the aggregate is first persisted.
    /// </summary>
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    ///     Gets or sets the timestamp captured when the aggregate is updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; set; }
}
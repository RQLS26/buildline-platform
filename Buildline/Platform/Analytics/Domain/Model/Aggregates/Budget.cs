using System.ComponentModel.DataAnnotations.Schema;
using Buildline.Platform.Analytics.Domain.Model.Commands;
using Buildline.Platform.Analytics.Domain.Model.Events;
using Buildline.Platform.Analytics.Domain.Model.ValueObjects;
using Buildline.Platform.Shared.Domain.Model.Entities;
using Buildline.Platform.Shared.Domain.Model.Events;

namespace Buildline.Platform.Analytics.Domain.Model.Aggregates;

/// <summary>
///     Aggregate root that represents the budget state of a construction project.
/// </summary>
public partial class Budget : IAuditableEntity, IHasDomainEvents
{
    private readonly List<IEvent> _domainEvents = [];

    /// <summary>Initializes an empty budget for Entity Framework Core materialization.</summary>
    protected Budget()
    {
        Project = string.Empty;
        Status = string.Empty;
    }

    /// <summary>Creates a budget aggregate from a budgeting command.</summary>
    /// <param name="command">Command carrying budget values accepted by the application layer.</param>
    public Budget(CreateBudgetCommand command)
    {
        CompanyId = command.CompanyId;
        Project = command.Project?.Trim() ?? string.Empty;
        TotalBudget = command.TotalBudget ?? 0m;
        Spent = command.Spent ?? 0m;
        Allocated = command.Allocated ?? 0m;
        Status = command.Status?.Trim() ?? BudgetHealth.OnTrack.ToString();
    }

    /// <summary>Gets the database-generated budget identifier.</summary>
    public int Id { get; private set; }

    /// <summary>Gets the company profile identifier that owns this operational record.</summary>
    public int CompanyId { get; private set; }

    /// <summary>Gets the project represented by this budget row.</summary>
    public string Project { get; private set; }

    /// <summary>Gets the total budget approved for the project.</summary>
    public decimal TotalBudget { get; private set; }

    /// <summary>Gets the actual amount spent.</summary>
    public decimal Spent { get; private set; }

    /// <summary>Gets the amount allocated to near-term procurement commitments.</summary>
    public decimal Allocated { get; private set; }

    /// <summary>Gets the calculated budget status displayed by dashboards.</summary>
    public string Status { get; private set; }

    /// <summary>Gets or sets the audit timestamp captured when the budget row is created.</summary>
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>Gets or sets the audit timestamp captured when the budget row is updated.</summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <inheritdoc />
    [NotMapped]
    public IReadOnlyCollection<IEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <inheritdoc />
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    /// <summary>Applies a partial budget update.</summary>
    /// <param name="command">Command containing replacement values.</param>
    public void Apply(UpdateBudgetCommand command)
    {
        var previousStatus = Status;
        Project = command.Project is null ? Project : command.Project.Trim();
        TotalBudget = command.TotalBudget ?? TotalBudget;
        Spent = command.Spent ?? Spent;
        Allocated = command.Allocated ?? Allocated;
        Status = command.Status is null ? Status : command.Status.Trim();

        if (!string.Equals(previousStatus, Status, StringComparison.OrdinalIgnoreCase))
            AddDomainEvent(new BudgetStatusChangedEvent(Id, Project, previousStatus, Status));
    }

    /// <summary>Records a domain event raised by this aggregate.</summary>
    /// <param name="domainEvent">Event that describes a completed domain change.</param>
    private void AddDomainEvent(IEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}

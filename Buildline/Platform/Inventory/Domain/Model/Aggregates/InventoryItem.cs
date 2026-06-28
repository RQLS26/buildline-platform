using Buildline.Platform.Shared.Domain.Model.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using Buildline.Platform.Inventory.Domain.Model.Commands;
using Buildline.Platform.Inventory.Domain.Model.Events;
using Buildline.Platform.Inventory.Domain.Model.ValueObjects;
using Buildline.Platform.Shared.Domain.Model.Events;

namespace Buildline.Platform.Inventory.Domain.Model.Aggregates;

/// <summary>
///     Aggregate root that represents a stock item controlled by the Inventory bounded context.
/// </summary>
public partial class InventoryItem : IHasDomainEvents
{
    private readonly List<IEvent> _domainEvents = [];

    /// <summary>Initializes an empty inventory item for Entity Framework Core materialization.</summary>
    protected InventoryItem()
    {
        Sku = string.Empty;
        Name = string.Empty;
        Project = string.Empty;
        Category = string.Empty;
        LastUpdated = string.Empty;
    }

    /// <summary>Creates an inventory item from a stock command.</summary>
    /// <param name="command">Command carrying inventory values accepted by the application layer.</param>
    public InventoryItem(CreateInventoryItemCommand command)
    {
        CompanyId = command.CompanyId;
        var stockLevel = StockLevel.From(command.CurrentStock, command.MinStock, command.MaxStock);
        Sku = command.Sku?.Trim() ?? string.Empty;
        Name = command.Name?.Trim() ?? string.Empty;
        Project = command.Project?.Trim() ?? string.Empty;
        Category = command.Category?.Trim() ?? string.Empty;
        CurrentStock = stockLevel.Current;
        MaxStock = stockLevel.Maximum;
        MinStock = stockLevel.Minimum;
        LastUpdated = command.LastUpdated?.Trim() ?? DateTime.UtcNow.ToString("yyyy-MM-dd");
    }

    /// <summary>Gets the database-generated inventory item identifier.</summary>
    public int Id { get; private set; }

    /// <summary>Gets the company profile identifier that owns this operational record.</summary>
    public int CompanyId { get; private set; }

    /// <summary>Gets the stock keeping unit displayed by inventory tables.</summary>
    public string Sku { get; private set; }

    /// <summary>Gets the inventory item name.</summary>
    public string Name { get; private set; }

    /// <summary>Gets the project where the stock is physically managed.</summary>
    public string Project { get; private set; }

    /// <summary>Gets the material category used by inventory filters.</summary>
    public string Category { get; private set; }

    /// <summary>Gets the current available stock.</summary>
    public int CurrentStock { get; private set; }

    /// <summary>Gets the maximum desired stock.</summary>
    public int MaxStock { get; private set; }

    /// <summary>Gets the minimum stock threshold.</summary>
    public int MinStock { get; private set; }

    /// <summary>Gets the last stock update date.</summary>
    public string LastUpdated { get; private set; }

    /// <inheritdoc />
    [NotMapped]
    public IReadOnlyCollection<IEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <inheritdoc />
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    /// <summary>Applies a partial stock update.</summary>
    /// <param name="command">Command containing replacement values.</param>
    public void Apply(UpdateInventoryItemCommand command)
    {
        var previousStock = CurrentStock;
        Sku = command.Sku is null ? Sku : command.Sku.Trim();
        Name = command.Name is null ? Name : command.Name.Trim();
        Project = command.Project is null ? Project : command.Project.Trim();
        Category = command.Category is null ? Category : command.Category.Trim();
        CurrentStock = command.CurrentStock ?? CurrentStock;
        MaxStock = command.MaxStock ?? MaxStock;
        MinStock = command.MinStock ?? MinStock;
        LastUpdated = command.LastUpdated is null ? LastUpdated : command.LastUpdated.Trim();

        var stockLevel = new StockLevel(CurrentStock, MinStock, MaxStock);
        if (previousStock != CurrentStock)
            AddDomainEvent(new InventoryStockChangedEvent(Id, Sku, previousStock, CurrentStock, stockLevel.IsBelowMinimum));
    }

    /// <summary>Records a domain event raised by this aggregate.</summary>
    /// <param name="domainEvent">Event that describes a completed domain change.</param>
    private void AddDomainEvent(IEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}

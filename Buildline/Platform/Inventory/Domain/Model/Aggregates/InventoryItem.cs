using Buildline.Platform.Inventory.Interfaces.Rest.Resources;
using Buildline.Platform.Shared.Domain.Model.Entities;

namespace Buildline.Platform.Inventory.Domain.Model.Aggregates;

/// <summary>
///     Aggregate root that represents a stock item controlled by the Inventory bounded context.
/// </summary>
/// <remarks>
///     Inventory items are separate from shared material catalog entries because they represent stock
///     levels, thresholds and warehouse state for a specific project.
/// </remarks>
public partial class InventoryItem : IAuditableEntity
{
    /// <summary>Initializes an empty inventory item for Entity Framework Core materialization.</summary>
    protected InventoryItem()
    {
        Sku = string.Empty;
        Name = string.Empty;
        Project = string.Empty;
        Category = string.Empty;
        LastUpdated = string.Empty;
    }

    /// <summary>Creates an inventory item from the frontend inventory contract.</summary>
    /// <param name="resource">Inventory payload submitted by the stock management screen.</param>
    public InventoryItem(InventoryItemWriteResource resource)
    {
        Sku = resource.Sku?.Trim() ?? string.Empty;
        Name = resource.Name?.Trim() ?? string.Empty;
        Project = resource.Project?.Trim() ?? string.Empty;
        Category = resource.Category?.Trim() ?? string.Empty;
        CurrentStock = resource.CurrentStock ?? 0;
        MaxStock = resource.MaxStock ?? 0;
        MinStock = resource.MinStock ?? 0;
        LastUpdated = resource.LastUpdated?.Trim() ?? DateTime.UtcNow.ToString("yyyy-MM-dd");
    }

    /// <summary>Gets the database-generated inventory item identifier.</summary>
    public int Id { get; private set; }

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

    /// <summary>Gets or sets the audit timestamp captured when the stock item is created.</summary>
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>Gets or sets the audit timestamp captured when the stock item is updated.</summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>Applies a partial stock update.</summary>
    /// <param name="resource">Inventory fields to replace.</param>
    public void Apply(InventoryItemWriteResource resource)
    {
        Sku = resource.Sku is null ? Sku : resource.Sku.Trim();
        Name = resource.Name is null ? Name : resource.Name.Trim();
        Project = resource.Project is null ? Project : resource.Project.Trim();
        Category = resource.Category is null ? Category : resource.Category.Trim();
        CurrentStock = resource.CurrentStock ?? CurrentStock;
        MaxStock = resource.MaxStock ?? MaxStock;
        MinStock = resource.MinStock ?? MinStock;
        LastUpdated = resource.LastUpdated is null ? LastUpdated : resource.LastUpdated.Trim();
    }
}

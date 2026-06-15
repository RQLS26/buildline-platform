using Buildline.Platform.Shared.Domain.Model.Entities;
using Buildline.Platform.Materials.Domain.Model.Commands;

namespace Buildline.Platform.Materials.Domain.Model.Aggregates;

/// <summary>
///     Material aggregate root for the Buildline catalog used by requisitions and inventory.
/// </summary>
public partial class Material : IAuditableEntity
{
    protected Material()
    {
        Sku = string.Empty;
        Name = string.Empty;
        Category = string.Empty;
        Unit = string.Empty;
        Project = string.Empty;
    }

    public Material(
        string sku,
        string name,
        string category,
        string unit,
        string project,
        int currentStock,
        int minStock,
        int maxStock)
    {
        Sku = sku;
        Name = name;
        Category = category;
        Unit = unit;
        Project = project;
        CurrentStock = currentStock;
        MinStock = minStock;
        MaxStock = maxStock;
    }

    public Material(CreateMaterialCommand command)
        : this(
            command.Sku,
            command.Name,
            command.Category,
            command.Unit,
            command.Project,
            command.CurrentStock,
            command.MinStock,
            command.MaxStock)
    {
    }

    public int Id { get; private set; }
    public string Sku { get; private set; }
    public string Name { get; private set; }
    public string Category { get; private set; }
    public string Unit { get; private set; }
    public string Project { get; private set; }
    public int CurrentStock { get; private set; }
    public int MinStock { get; private set; }
    public int MaxStock { get; private set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}

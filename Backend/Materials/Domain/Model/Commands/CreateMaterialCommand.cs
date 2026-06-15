namespace Buildline.Platform.Materials.Domain.Model.Commands;

public record CreateMaterialCommand(
    string Sku,
    string Name,
    string Category,
    string Unit,
    string Project,
    int CurrentStock,
    int MinStock,
    int MaxStock);

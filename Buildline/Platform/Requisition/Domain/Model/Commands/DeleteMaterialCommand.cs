namespace Buildline.Platform.Requisition.Domain.Model.Commands;

/// <summary>
///     Command that requests deletion of a material from material references.
/// </summary>
/// <param name="MaterialId">Identifier of the material aggregate that must be removed.</param>
/// <remarks>
///     This command supports TS-08 and keeps deletion behavior in the application command layer.
/// </remarks>
public record DeleteMaterialCommand(int MaterialId);



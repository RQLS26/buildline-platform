namespace Buildline.Platform.Requisition.Domain.Model.ValueObjects;

/// <summary>
///     Value object that represents the material requested by a requisition or available in the material endpoint.
/// </summary>
/// <param name="Name">Material display name used by requisition and inventory workflows.</param>
/// <param name="Unit">Measurement unit used to request or count the material.</param>
/// <param name="Category">Material category used by frontend filters.</param>
public readonly record struct MaterialDescriptor(string Name, string Unit, string Category)
{
    /// <summary>Creates a normalized descriptor from nullable write values.</summary>
    public static MaterialDescriptor From(string? name, string? unit, string? category)
    {
        return new MaterialDescriptor((name ?? string.Empty).Trim(), (unit ?? string.Empty).Trim(), (category ?? string.Empty).Trim());
    }
}

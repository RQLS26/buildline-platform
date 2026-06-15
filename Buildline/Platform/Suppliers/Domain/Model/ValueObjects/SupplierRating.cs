namespace Buildline.Platform.Suppliers.Domain.Model.ValueObjects;

/// <summary>Value object that constrains supplier rating to the frontend-supported one-to-five scale.</summary>
/// <param name="Value">Rating value clamped between one and five.</param>
public readonly record struct SupplierRating(int Value)
{
    /// <summary>Creates a normalized supplier rating.</summary>
    public static SupplierRating From(int? value)
    {
        return new SupplierRating(Math.Clamp(value ?? 3, 1, 5));
    }
}

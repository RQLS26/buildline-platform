namespace Buildline.Platform.Inventory.Domain.Model.ValueObjects;

/// <summary>Value object that groups current, minimum and maximum stock quantities.</summary>
/// <param name="Current">Current available stock.</param>
/// <param name="Minimum">Minimum desired stock threshold.</param>
/// <param name="Maximum">Maximum desired stock threshold.</param>
public readonly record struct StockLevel(int Current, int Minimum, int Maximum)
{
    /// <summary>Builds normalized stock values from nullable command fields.</summary>
    public static StockLevel From(int? current, int? minimum, int? maximum)
    {
        return new StockLevel(Math.Max(0, current ?? 0), Math.Max(0, minimum ?? 0), Math.Max(0, maximum ?? 0));
    }

    /// <summary>Gets whether current stock is below the configured minimum threshold.</summary>
    public bool IsBelowMinimum => Current < Minimum;
}

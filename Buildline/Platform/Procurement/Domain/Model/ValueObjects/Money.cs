namespace Buildline.Platform.Procurement.Domain.Model.ValueObjects;

/// <summary>Value object that represents a non-negative monetary amount in the procurement workflow.</summary>
/// <param name="Amount">Decimal amount stored by the current frontend-compatible API contract.</param>
public readonly record struct Money(decimal Amount)
{
    /// <summary>Creates a money value while preventing negative operational amounts.</summary>
    public static Money From(decimal? amount)
    {
        return new Money(Math.Max(0m, amount ?? 0m));
    }
}

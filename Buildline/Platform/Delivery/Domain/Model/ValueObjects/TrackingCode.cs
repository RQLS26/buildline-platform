namespace Buildline.Platform.Delivery.Domain.Model.ValueObjects;

/// <summary>Value object for delivery tracking codes shown in logistics workflows.</summary>
/// <param name="Value">Normalized tracking code.</param>
public readonly record struct TrackingCode(string Value)
{
    /// <summary>Creates a tracking code or generates a deterministic prefix for new deliveries.</summary>
    public static TrackingCode From(string? value)
    {
        return new TrackingCode(string.IsNullOrWhiteSpace(value) ? $"TRK-{DateTime.UtcNow:HHmmss}" : value.Trim());
    }
}

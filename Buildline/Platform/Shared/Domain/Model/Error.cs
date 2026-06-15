namespace Buildline.Platform.Shared.Domain.Model;

/// <summary>
///     Represents a domain error.
/// </summary>
/// <param name="Code">The unique error code.</param>
/// <param name="Message">The error message.</param>
public record Error(string Code, string Message)
{
    /// <summary>
    ///     Empty error value used when no error exists.
    /// </summary>
    public static readonly Error None = new(string.Empty, string.Empty);

    /// <summary>
    ///     Error value used when a required result value is unexpectedly null.
    /// </summary>
    public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.");
}

namespace Buildline.Platform.Iam.Infrastructure.Tokens.Jwt.Configuration;

/// <summary>
///     Configuration values used by JWT token generation and validation.
/// </summary>
public class TokenSettings
{
    /// <summary>
    ///     Gets or sets the symmetric signing secret used for HMAC SHA-256 JWT tokens.
    /// </summary>
    /// <remarks>
    ///     Production configuration should provide this value through the
    ///     <c>BUILDLINE_JWT_SECRET</c> environment variable referenced by appsettings.
    /// </remarks>
    public string Secret { get; set; } = string.Empty;
}

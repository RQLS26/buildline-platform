using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Buildline.Platform.Iam.Infrastructure.Tokens.Jwt.Services;

/// <summary>
///     Creates the symmetric signing key used by the platform JWT infrastructure.
/// </summary>
/// <remarks>
///     ASP.NET Core and the token generator must use the exact same key material. The factory
///     centralizes that transformation and derives a fixed 256-bit key with SHA-256 so Railway
///     secrets, local development secrets and future deployment-provider secrets are interpreted
///     consistently by every authentication component.
/// </remarks>
public static class TokenSigningKeyFactory
{
    /// <summary>
    ///     Builds the symmetric security key used to sign and validate HMAC SHA-256 JWTs.
    /// </summary>
    /// <param name="secret">Configured JWT secret read from <c>TokenSettings:Secret</c>.</param>
    /// <returns>A 256-bit symmetric security key derived from the configured secret.</returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the configured secret is missing or blank.
    /// </exception>
    public static SymmetricSecurityKey CreateFromSecret(string secret)
    {
        if (string.IsNullOrWhiteSpace(secret))
            throw new InvalidOperationException("JWT token secret is not set in TokenSettings.");

        var secretBytes = Encoding.UTF8.GetBytes(secret);
        var signingKeyBytes = SHA256.HashData(secretBytes);
        return new SymmetricSecurityKey(signingKeyBytes);
    }
}

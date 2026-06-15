using Buildline.Platform.Iam.Domain.Model.Aggregates;

namespace Buildline.Platform.Iam.Application.Internal.OutboundServices;

/// <summary>
///     Defines token operations required by the IAM application layer.
/// </summary>
/// <remarks>
///     The application layer depends on this abstraction instead of JWT infrastructure details so
///     token generation and validation can evolve without changing command services.
/// </remarks>
public interface ITokenService
{
    /// <summary>
    ///     Generates an access token for an authenticated user.
    /// </summary>
    /// <param name="user">User aggregate whose identity and role are encoded as claims.</param>
    /// <returns>A signed JWT access token.</returns>
    string GenerateToken(User user);

    /// <summary>
    ///     Validates an access token and extracts the user identifier claim.
    /// </summary>
    /// <param name="token">JWT token received from an Authorization header.</param>
    /// <returns>The user identifier when validation succeeds; otherwise <c>null</c>.</returns>
    Task<int?> ValidateToken(string token);
}

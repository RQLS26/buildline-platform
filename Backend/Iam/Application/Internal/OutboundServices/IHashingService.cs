namespace Buildline.Platform.Iam.Application.Internal.OutboundServices;

/// <summary>
///     Defines password hashing operations required by the IAM application layer.
/// </summary>
/// <remarks>
///     The command service depends on this abstraction so credential hashing remains replaceable and
///     domain aggregates never receive plain text passwords.
/// </remarks>
public interface IHashingService
{
    /// <summary>
    ///     Hashes a plain text password for secure persistence.
    /// </summary>
    /// <param name="password">Plain text password received from a trusted application command.</param>
    /// <returns>A BCrypt password hash that can be stored in the users table.</returns>
    string HashPassword(string password);

    /// <summary>
    ///     Verifies a plain text password against a stored hash.
    /// </summary>
    /// <param name="password">Plain text password submitted during sign-in.</param>
    /// <param name="passwordHash">Stored BCrypt hash retrieved from persistence.</param>
    /// <returns><c>true</c> when the password matches the hash; otherwise <c>false</c>.</returns>
    bool VerifyPassword(string password, string passwordHash);
}

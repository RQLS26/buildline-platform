using Buildline.Platform.Iam.Application.Internal.OutboundServices;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Buildline.Platform.Iam.Infrastructure.Hashing.BCrypt.Services;

/// <summary>
///     BCrypt implementation of password hashing operations.
/// </summary>
/// <remarks>
///     BCrypt is used because IAM must persist irreversible password hashes while still allowing
///     credential verification during sign-in.
/// </remarks>
public class HashingService : IHashingService
{
    /// <inheritdoc />
    public string HashPassword(string password)
    {
        return BCryptNet.HashPassword(password);
    }

    /// <inheritdoc />
    public bool VerifyPassword(string password, string passwordHash)
    {
        return BCryptNet.Verify(password, passwordHash);
    }
}

using Buildline.Platform.Iam.Domain.Model.Aggregates;
using Buildline.Platform.Shared.Domain.Repositories;

namespace Buildline.Platform.Iam.Domain.Repositories;

/// <summary>
///     Repository contract for IAM user aggregate persistence.
/// </summary>
/// <remarks>
///     The interface extends the shared repository with email-based lookup operations required by
///     authentication and uniqueness validation.
/// </remarks>
public interface IUserRepository : IBaseRepository<User>
{
    /// <summary>
    ///     Finds a user by email.
    /// </summary>
    /// <param name="email">Email used as the lookup value.</param>
    /// <param name="cancellationToken">Token used to cancel database access.</param>
    /// <returns>The user when the email exists; otherwise <c>null</c>.</returns>
    Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Determines whether a user email is already registered.
    /// </summary>
    /// <param name="email">Email used as the uniqueness value.</param>
    /// <param name="cancellationToken">Token used to cancel database access.</param>
    /// <returns><c>true</c> when the email exists; otherwise <c>false</c>.</returns>
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
}

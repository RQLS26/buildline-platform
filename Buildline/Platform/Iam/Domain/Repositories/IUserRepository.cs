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
    /// <summary>
    ///     Lists users assigned to the supplied company profile.
    /// </summary>
    /// <param name="companyId">Company profile identifier that scopes user membership.</param>
    /// <param name="cancellationToken">Token used to cancel the database query.</param>
    /// <returns>Users assigned to the company, or an empty collection.</returns>
    Task<IEnumerable<User>> ListByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Finds one user by persistence identity and company membership.
    /// </summary>
    /// <param name="id">User identifier requested by the API client.</param>
    /// <param name="companyId">Company profile identifier that must own the membership.</param>
    /// <param name="cancellationToken">Token used to cancel the database query.</param>
    /// <returns>The user when found in the company; otherwise <c>null</c>.</returns>
    Task<User?> FindByIdAndCompanyIdAsync(int id, int companyId, CancellationToken cancellationToken = default);
}

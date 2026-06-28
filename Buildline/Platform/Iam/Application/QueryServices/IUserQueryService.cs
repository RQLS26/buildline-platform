using Buildline.Platform.Iam.Domain.Model.Aggregates;

namespace Buildline.Platform.Iam.Application.QueryServices;

/// <summary>
///     Defines read operations exposed by the IAM application layer.
/// </summary>
/// <remarks>
///     Query services keep user lookup and listing use cases separate from sign-in, sign-up and user
///     management commands.
/// </remarks>
public interface IUserQueryService
{
    /// <summary>
    ///     Lists every registered user.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel repository access.</param>
    /// <returns>The registered IAM users.</returns>
    Task<IEnumerable<User>> ListAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Finds one user by identifier.
    /// </summary>
    /// <param name="userId">The identifier of the user to look up.</param>
    /// <param name="cancellationToken">Token used to cancel repository access.</param>
    /// <returns>The user when it exists; otherwise <c>null</c>.</returns>
    Task<User?> FindByIdAsync(int userId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Finds one user by email address.
    /// </summary>
    /// <param name="email">The email address to look up.</param>
    /// <param name="cancellationToken">Token used to cancel repository access.</param>
    /// <returns>The user when the email exists; otherwise <c>null</c>.</returns>
    Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken = default);
    /// <summary>
    ///     Lists users assigned to a company profile.
    /// </summary>
    /// <param name="companyId">Company profile identifier that scopes the users.</param>
    /// <param name="cancellationToken">Token used to cancel repository access.</param>
    /// <returns>Users assigned to the company.</returns>
    Task<IEnumerable<User>> ListByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Finds one user by identifier and company membership.
    /// </summary>
    /// <param name="userId">User identifier requested by the API client.</param>
    /// <param name="companyId">Company profile identifier that must own the membership.</param>
    /// <param name="cancellationToken">Token used to cancel repository access.</param>
    /// <returns>The user when found in the company; otherwise <c>null</c>.</returns>
    Task<User?> FindByIdAndCompanyIdAsync(int userId, int companyId, CancellationToken cancellationToken = default);
}

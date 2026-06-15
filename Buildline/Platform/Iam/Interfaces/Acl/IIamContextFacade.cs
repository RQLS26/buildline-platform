namespace Buildline.Platform.Iam.Interfaces.Acl;

/// <summary>
///     Anti-corruption facade exposed by the IAM bounded context.
/// </summary>
/// <remarks>
///     Other bounded contexts should use this facade when they need identity metadata. The facade
///     returns primitive values instead of IAM aggregates so external contexts do not depend on IAM
///     internals such as password hashes, token settings or persistence mappings.
/// </remarks>
public interface IIamContextFacade
{
    /// <summary>
    ///     Fetches a user identifier by email.
    /// </summary>
    /// <param name="email">Email used as the IAM lookup value.</param>
    /// <param name="cancellationToken">Token used to cancel the query.</param>
    /// <returns>The user id when the email exists; otherwise <c>0</c>.</returns>
    Task<int> FetchUserIdByEmail(string email, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Fetches a user email by identifier.
    /// </summary>
    /// <param name="userId">Identifier of the user requested by another bounded context.</param>
    /// <param name="cancellationToken">Token used to cancel the query.</param>
    /// <returns>The user email when the user exists; otherwise an empty string.</returns>
    Task<string> FetchUserEmailById(int userId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Fetches a user role by identifier.
    /// </summary>
    /// <param name="userId">Identifier of the user requested by another bounded context.</param>
    /// <param name="cancellationToken">Token used to cancel the query.</param>
    /// <returns>The user role when the user exists; otherwise an empty string.</returns>
    Task<string> FetchUserRoleById(int userId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Determines whether a user email is already registered.
    /// </summary>
    /// <param name="email">Email used as the uniqueness lookup value.</param>
    /// <param name="cancellationToken">Token used to cancel the query.</param>
    /// <returns><c>true</c> when the email exists; otherwise <c>false</c>.</returns>
    Task<bool> ExistsUserByEmail(string email, CancellationToken cancellationToken = default);
}

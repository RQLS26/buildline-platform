using Buildline.Platform.Iam.Domain.Model.Aggregates;
using Buildline.Platform.Iam.Domain.Model.Queries;

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
    ///     Handles the query that retrieves every registered user.
    /// </summary>
    /// <param name="query">Query object representing the users listing request.</param>
    /// <param name="cancellationToken">Token used to cancel repository access.</param>
    /// <returns>The registered IAM users.</returns>
    Task<IEnumerable<User>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handles the query that retrieves one user by identifier.
    /// </summary>
    /// <param name="query">Query object containing the requested user id.</param>
    /// <param name="cancellationToken">Token used to cancel repository access.</param>
    /// <returns>The user when it exists; otherwise <c>null</c>.</returns>
    Task<User?> Handle(GetUserByIdQuery query, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handles the query that retrieves one user by email.
    /// </summary>
    /// <param name="query">Query object containing the requested email.</param>
    /// <param name="cancellationToken">Token used to cancel repository access.</param>
    /// <returns>The user when the email exists; otherwise <c>null</c>.</returns>
    Task<User?> Handle(GetUserByEmailQuery query, CancellationToken cancellationToken = default);
}

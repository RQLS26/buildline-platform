using Buildline.Platform.Iam.Application.QueryServices;
using Buildline.Platform.Iam.Domain.Model.Aggregates;
using Buildline.Platform.Iam.Domain.Model.Queries;
using Buildline.Platform.Iam.Domain.Repositories;

namespace Buildline.Platform.Iam.Application.Internal.QueryServices;

/// <summary>
///     Application query service that coordinates read access to IAM users.
/// </summary>
/// <param name="userRepository">Repository used to retrieve persisted user aggregates.</param>
/// <remarks>
///     The service supports authentication lookups and the users-management API while keeping
///     controllers independent from Entity Framework Core.
/// </remarks>
public class UserQueryService(IUserRepository userRepository) : IUserQueryService
{
    /// <summary>
    ///     Retrieves every registered user.
    /// </summary>
    /// <param name="query">Users listing query.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>A collection of user aggregates, possibly empty.</returns>
    public async Task<IEnumerable<User>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken = default)
    {
        return await userRepository.ListAsync(cancellationToken);
    }

    /// <summary>
    ///     Retrieves one user by identifier.
    /// </summary>
    /// <param name="query">User lookup query containing the requested id.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>The user aggregate when found; otherwise <c>null</c>.</returns>
    public async Task<User?> Handle(GetUserByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await userRepository.FindByIdAsync(query.UserId, cancellationToken);
    }

    /// <summary>
    ///     Retrieves one user by email.
    /// </summary>
    /// <param name="query">User lookup query containing the requested email.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>The user aggregate when found; otherwise <c>null</c>.</returns>
    public async Task<User?> Handle(GetUserByEmailQuery query, CancellationToken cancellationToken = default)
    {
        return await userRepository.FindByEmailAsync(query.Email, cancellationToken);
    }
}

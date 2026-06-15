using Buildline.Platform.Iam.Domain.Model.Aggregates;
using Buildline.Platform.Iam.Domain.Model.Commands;
using Buildline.Platform.Shared.Application.Model;

namespace Buildline.Platform.Iam.Application.CommandServices;

/// <summary>
///     Defines the application command operations owned by the IAM bounded context.
/// </summary>
/// <remarks>
///     Command services coordinate repositories, hashing, token generation and unit-of-work commits.
///     Controllers depend on this contract instead of infrastructure details so each Sprint 3 REST
///     endpoint remains aligned with CQRS boundaries.
/// </remarks>
public interface IUserCommandService
{
    /// <summary>
    ///     Creates a user from the user-management module.
    /// </summary>
    /// <param name="command">Command containing account metadata and the plain password to hash.</param>
    /// <param name="cancellationToken">Token used to cancel database and hashing-adjacent work.</param>
    /// <returns>A result containing the created user or a domain error such as duplicated email.</returns>
    Task<Result<User>> Handle(CreateUserCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Updates an existing user's administrative fields.
    /// </summary>
    /// <param name="command">Command with the target user id and replacement account metadata.</param>
    /// <param name="cancellationToken">Token used to cancel repository and unit-of-work operations.</param>
    /// <returns>A result containing the updated user or an IAM error describing why the update failed.</returns>
    Task<Result<User>> Handle(UpdateUserCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Authenticates an existing active user and issues a JWT token.
    /// </summary>
    /// <param name="command">Command carrying email and password credentials.</param>
    /// <param name="cancellationToken">Token used to cancel repository access.</param>
    /// <returns>A result containing the authenticated user and generated token.</returns>
    Task<Result<(User user, string token)>> Handle(SignInCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Registers a new user from the public sign-up workflow and issues a JWT token.
    /// </summary>
    /// <param name="command">Command carrying registration data accepted by the IAM endpoint.</param>
    /// <param name="cancellationToken">Token used to cancel persistence operations.</param>
    /// <returns>A result containing the created user and generated token.</returns>
    Task<Result<(User user, string token)>> Handle(SignUpCommand command, CancellationToken cancellationToken = default);
}

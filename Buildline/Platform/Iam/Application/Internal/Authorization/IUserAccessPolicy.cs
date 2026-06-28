using Buildline.Platform.Iam.Domain.Model.Aggregates;
using Buildline.Platform.Iam.Interfaces.Rest.Resources;

namespace Buildline.Platform.Iam.Application.Internal.Authorization;

/// <summary>
///     Defines authorization decisions for IAM user-management use cases.
/// </summary>
public interface IUserAccessPolicy
{
    /// <summary>
    ///     Determines whether the actor can list users in the specified company.
    /// </summary>
    /// <param name="actor">Authenticated user resolved from the JWT session.</param>
    /// <param name="companyId">Company identifier from the route.</param>
    /// <returns><c>true</c> when the actor can list company users; otherwise <c>false</c>.</returns>
    bool CanListCompanyUsers(User? actor, int companyId);

    /// <summary>
    ///     Determines whether the actor can create users in the specified company.
    /// </summary>
    /// <param name="actor">Authenticated user resolved from the JWT session.</param>
    /// <param name="companyId">Company identifier from the route.</param>
    /// <returns><c>true</c> when the actor can create users; otherwise <c>false</c>.</returns>
    bool CanCreateUser(User? actor, int companyId);

    /// <summary>
    ///     Determines whether the actor can read one target user inside the specified company.
    /// </summary>
    /// <param name="actor">Authenticated user resolved from the JWT session.</param>
    /// <param name="targetUser">Target user requested by the API client.</param>
    /// <param name="companyId">Company identifier from the route.</param>
    /// <returns><c>true</c> when the actor can read the target user; otherwise <c>false</c>.</returns>
    bool CanReadUser(User? actor, User targetUser, int companyId);

    /// <summary>
    ///     Determines whether the actor can update the target user with the requested patch fields.
    /// </summary>
    /// <param name="actor">Authenticated user resolved from the JWT session.</param>
    /// <param name="targetUser">Target user being patched.</param>
    /// <param name="targetUserId">Identifier from the route.</param>
    /// <param name="resource">Patch payload submitted by the frontend.</param>
    /// <returns><c>true</c> when the update is allowed; otherwise <c>false</c>.</returns>
    bool CanUpdateUser(User? actor, User targetUser, int targetUserId, UpdateUserResource resource);

    /// <summary>
    ///     Determines whether the authenticated user can change the target account password.
    /// </summary>
    /// <param name="actorUserId">Authenticated user identifier from the JWT session.</param>
    /// <param name="targetUserId">Target user identifier from the route.</param>
    /// <returns><c>true</c> when the password change is self-service; otherwise <c>false</c>.</returns>
    bool CanChangePassword(int? actorUserId, int targetUserId);
}
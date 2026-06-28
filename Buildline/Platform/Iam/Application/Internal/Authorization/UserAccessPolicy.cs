using Buildline.Platform.Iam.Domain.Model.Aggregates;
using Buildline.Platform.Iam.Interfaces.Rest.Resources;

namespace Buildline.Platform.Iam.Application.Internal.Authorization;

/// <summary>
///     Centralizes IAM user-management authorization rules used by REST endpoints.
/// </summary>
/// <remarks>
///     The policy keeps company membership and sensitive role/status changes out of controllers while
///     preserving the same behavior expected by the current users administration screen.
/// </remarks>
public class UserAccessPolicy : IUserAccessPolicy
{
    /// <inheritdoc />
    public bool CanListCompanyUsers(User? actor, int companyId)
    {
        return IsAssignedToCompany(actor, companyId) && IsOwnerOrAdmin(actor);
    }

    /// <inheritdoc />
    public bool CanCreateUser(User? actor, int companyId)
    {
        return IsAssignedToCompany(actor, companyId) && IsOwner(actor);
    }

    /// <inheritdoc />
    public bool CanReadUser(User? actor, User targetUser, int companyId)
    {
        return IsAssignedToCompany(actor, companyId) && targetUser.CompanyId == companyId && IsOwnerOrAdmin(actor);
    }

    /// <inheritdoc />
    public bool CanUpdateUser(User? actor, User targetUser, int targetUserId, UpdateUserResource resource)
    {
        if (actor is null)
            return false;

        var isSelf = actor.Id == targetUserId;
        var isOwner = IsOwner(actor);
        var changesRoleOrStatus = resource.Role is not null || resource.IsActive is not null;
        var changesMembership = resource.CompanyId is not null || resource.MembershipStatus is not null;

        if (targetUser.CompanyId != actor.CompanyId && !isSelf)
            return false;

        if (changesMembership && (!isOwner || isSelf))
            return false;

        if (changesRoleOrStatus)
            return isOwner && !isSelf;

        return isSelf || isOwner;
    }

    /// <inheritdoc />
    public bool CanChangePassword(int? actorUserId, int targetUserId)
    {
        return actorUserId == targetUserId;
    }

    private static bool IsAssignedToCompany(User? actor, int companyId)
    {
        return actor?.CompanyId == companyId;
    }

    private static bool IsOwnerOrAdmin(User? actor)
    {
        return IsOwner(actor) || string.Equals(actor?.Role, "admin", StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsOwner(User? actor)
    {
        return string.Equals(actor?.Role, "owner", StringComparison.OrdinalIgnoreCase);
    }
}
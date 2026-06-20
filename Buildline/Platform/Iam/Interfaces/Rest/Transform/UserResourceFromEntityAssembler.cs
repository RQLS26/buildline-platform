using Buildline.Platform.Iam.Domain.Model.Aggregates;
using Buildline.Platform.Iam.Interfaces.Rest.Resources;

namespace Buildline.Platform.Iam.Interfaces.Rest.Transform;

/// <summary>
///     Assembler that converts IAM user aggregates into users-management REST resources.
/// </summary>
/// <remarks>
///     The assembler intentionally excludes password hashes from the response contract.
/// </remarks>
public static class UserResourceFromEntityAssembler
{
    /// <summary>
    ///     Converts a user aggregate to the resource returned by users endpoints.
    /// </summary>
    /// <param name="user">User aggregate retrieved from persistence.</param>
    /// <returns>A frontend-compatible user resource without credential data.</returns>
    public static UserResource ToResourceFromEntity(User user)
    {
        return new UserResource(
            user.Id,
            user.Name,
            user.Email,
            user.Role,
            user.Department,
            user.Phone,
            user.AvatarColor,
            user.IsActive,
            user.LastLogin,
            user.TwoFactorEnabled,
            user.CompanyId,
            user.MembershipStatus);
    }
}

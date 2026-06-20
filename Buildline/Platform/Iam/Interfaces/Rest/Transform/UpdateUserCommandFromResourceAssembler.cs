using Buildline.Platform.Iam.Domain.Model.Aggregates;
using Buildline.Platform.Iam.Domain.Model.Commands;
using Buildline.Platform.Iam.Interfaces.Rest.Resources;

namespace Buildline.Platform.Iam.Interfaces.Rest.Transform;

/// <summary>
///     Assembler that translates a partial REST update resource into an IAM update command.
/// </summary>
/// <remarks>
///     The domain command requires complete replacement values, while the HTTP PATCH resource allows
///     partial fields. This assembler is the boundary that merges request values with the currently
///     persisted aggregate so the application layer receives a deterministic command.
/// </remarks>
public static class UpdateUserCommandFromResourceAssembler
{
    /// <summary>
    ///     Builds an update command using request values when present and current aggregate values otherwise.
    /// </summary>
    /// <param name="userId">Identifier extracted from the route and used as the command target.</param>
    /// <param name="currentUser">Current persisted user used to preserve omitted PATCH fields.</param>
    /// <param name="resource">Partial update resource received from the frontend users module.</param>
    /// <returns>An update command with complete values ready for application validation.</returns>
    public static UpdateUserCommand ToCommandFromResource(int userId, User currentUser, UpdateUserResource resource)
    {
        return new UpdateUserCommand(
            userId,
            resource.Name ?? currentUser.Name,
            resource.Email ?? currentUser.Email,
            resource.Role ?? currentUser.Role,
            resource.Department ?? currentUser.Department,
            resource.Phone ?? currentUser.Phone,
            resource.AvatarColor ?? currentUser.AvatarColor,
            resource.IsActive ?? currentUser.IsActive,
            resource.TwoFactorEnabled ?? currentUser.TwoFactorEnabled,
            resource.CompanyId ?? currentUser.CompanyId,
            resource.MembershipStatus ?? currentUser.MembershipStatus);
    }
}

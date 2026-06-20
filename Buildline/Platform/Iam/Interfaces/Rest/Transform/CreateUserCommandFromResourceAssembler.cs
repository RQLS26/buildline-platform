using Buildline.Platform.Iam.Domain.Model.Commands;
using Buildline.Platform.Iam.Interfaces.Rest.Resources;

namespace Buildline.Platform.Iam.Interfaces.Rest.Transform;

/// <summary>
///     Assembler that translates create-user REST resources into IAM commands.
/// </summary>
/// <remarks>
///     This assembler supports the users-management module and keeps frontend-specific payload shape
///     outside the IAM command service.
/// </remarks>
public static class CreateUserCommandFromResourceAssembler
{
    /// <summary>
    ///     Builds a create-user command from the administration request resource.
    /// </summary>
    /// <param name="resource">Resource received by the create-user endpoint.</param>
    /// <returns>A create-user command ready for validation, hashing and persistence.</returns>
    public static CreateUserCommand ToCommandFromResource(CreateUserResource resource)
    {
        return new CreateUserCommand(
            resource.Name,
            resource.Email,
            resource.Password,
            string.IsNullOrWhiteSpace(resource.Role) ? "viewer" : resource.Role,
            string.IsNullOrWhiteSpace(resource.Department) ? "General" : resource.Department,
            resource.Phone,
            string.IsNullOrWhiteSpace(resource.AvatarColor) ? "#3d63a1" : resource.AvatarColor,
            resource.IsActive,
            "Never",
            resource.CompanyId,
            string.IsNullOrWhiteSpace(resource.MembershipStatus) ? "active" : resource.MembershipStatus);
    }
}

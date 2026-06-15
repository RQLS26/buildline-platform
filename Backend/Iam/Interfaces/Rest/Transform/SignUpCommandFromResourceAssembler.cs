using Buildline.Platform.Iam.Domain.Model.Commands;
using Buildline.Platform.Iam.Interfaces.Rest.Resources;

namespace Buildline.Platform.Iam.Interfaces.Rest.Transform;

/// <summary>
///     Assembler that translates sign-up REST resources into IAM commands.
/// </summary>
public static class SignUpCommandFromResourceAssembler
{
    /// <summary>
    ///     Builds a sign-up command from the registration request resource.
    /// </summary>
    /// <param name="resource">Resource received by the sign-up endpoint.</param>
    /// <returns>A sign-up command ready for uniqueness validation and hashing.</returns>
    public static SignUpCommand ToCommandFromResource(SignUpResource resource)
    {
        return new SignUpCommand(
            resource.Name,
            resource.Email,
            resource.Password,
            string.IsNullOrWhiteSpace(resource.Role) ? "viewer" : resource.Role,
            string.IsNullOrWhiteSpace(resource.Department) ? "General" : resource.Department,
            resource.Phone,
            string.IsNullOrWhiteSpace(resource.AvatarColor) ? "#3d63a1" : resource.AvatarColor);
    }
}

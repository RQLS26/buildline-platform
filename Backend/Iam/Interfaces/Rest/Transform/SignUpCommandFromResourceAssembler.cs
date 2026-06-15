using Buildline.Platform.Iam.Domain.Model.Commands;
using Buildline.Platform.Iam.Interfaces.Rest.Resources;

namespace Buildline.Platform.Iam.Interfaces.Rest.Transform;

public static class SignUpCommandFromResourceAssembler
{
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

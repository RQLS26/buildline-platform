using Buildline.Platform.Iam.Domain.Model.Aggregates;
using Buildline.Platform.Iam.Domain.Model.Commands;
using Buildline.Platform.Iam.Interfaces.Rest.Resources;

namespace Buildline.Platform.Iam.Interfaces.Rest.Transform;

public static class UpdateUserCommandFromResourceAssembler
{
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
            resource.IsActive ?? currentUser.IsActive);
    }
}

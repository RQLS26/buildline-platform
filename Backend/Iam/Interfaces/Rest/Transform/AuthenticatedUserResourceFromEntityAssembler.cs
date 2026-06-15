using Buildline.Platform.Iam.Domain.Model.Aggregates;
using Buildline.Platform.Iam.Interfaces.Rest.Resources;

namespace Buildline.Platform.Iam.Interfaces.Rest.Transform;

/// <summary>
///     Assembler that converts authenticated user data into a session REST resource.
/// </summary>
public static class AuthenticatedUserResourceFromEntityAssembler
{
    /// <summary>
    ///     Converts a user aggregate and JWT token to the authentication response resource.
    /// </summary>
    /// <param name="user">Authenticated user aggregate.</param>
    /// <param name="token">JWT access token generated for the user.</param>
    /// <returns>A frontend-compatible authenticated user resource.</returns>
    public static AuthenticatedUserResource ToResourceFromEntity(User user, string token)
    {
        return new AuthenticatedUserResource(
            user.Id,
            user.Name,
            user.Email,
            user.Role,
            user.Department,
            user.Phone,
            user.AvatarColor,
            user.IsActive,
            user.LastLogin,
            token);
    }
}

namespace Buildline.Platform.Iam.Interfaces.Rest.Resources;

public record CreateUserResource(
    string Name,
    string Email,
    string Password,
    string Role,
    string Department,
    string Phone,
    string AvatarColor,
    bool IsActive);

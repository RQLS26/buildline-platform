namespace Buildline.Platform.Iam.Interfaces.Rest.Resources;

public record UserResource(
    int Id,
    string Name,
    string Email,
    string Role,
    string Department,
    string Phone,
    string AvatarColor,
    bool IsActive,
    string LastLogin);

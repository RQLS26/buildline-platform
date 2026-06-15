namespace Buildline.Platform.Iam.Interfaces.Rest.Resources;

public record UpdateUserResource(
    string? Name,
    string? Email,
    string? Role,
    string? Department,
    string? Phone,
    string? AvatarColor,
    bool? IsActive);

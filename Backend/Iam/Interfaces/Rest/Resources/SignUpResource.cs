namespace Buildline.Platform.Iam.Interfaces.Rest.Resources;

public record SignUpResource(
    string Name,
    string Email,
    string Password,
    string Role,
    string Department,
    string Phone,
    string AvatarColor);

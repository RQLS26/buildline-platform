namespace Buildline.Platform.Iam.Domain.Model.Commands;

public record UpdateUserCommand(
    int UserId,
    string Name,
    string Email,
    string Role,
    string Department,
    string Phone,
    string AvatarColor,
    bool IsActive);

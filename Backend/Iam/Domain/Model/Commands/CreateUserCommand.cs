namespace Buildline.Platform.Iam.Domain.Model.Commands;

public record CreateUserCommand(
    string Name,
    string Email,
    string Password,
    string Role,
    string Department,
    string Phone,
    string AvatarColor,
    bool IsActive,
    string LastLogin);

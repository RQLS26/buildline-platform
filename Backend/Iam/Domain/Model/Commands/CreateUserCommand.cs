namespace Buildline.Platform.Iam.Domain.Model.Commands;

/// <summary>
///     Command that requests creation of a user from the administration module.
/// </summary>
/// <param name="Name">Display name shown in user-management views.</param>
/// <param name="Email">Unique email used as the sign-in identifier.</param>
/// <param name="Password">Plain text initial password that must be hashed by the command service.</param>
/// <param name="Role">Application role assigned to the user.</param>
/// <param name="Department">Operational department associated with the user.</param>
/// <param name="Phone">Contact phone shown by the frontend when available.</param>
/// <param name="AvatarColor">Frontend avatar fallback color token.</param>
/// <param name="IsActive">Flag that determines whether the account can authenticate.</param>
/// <param name="LastLogin">Initial last-login display value.</param>
/// <remarks>
///     This command supports US-022 and keeps the users-management REST payload outside the domain
///     aggregate constructor.
/// </remarks>
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

namespace Buildline.Platform.Iam.Domain.Model.Commands;

/// <summary>
///     Command that requests public account registration.
/// </summary>
/// <param name="Name">Display name for the registered user.</param>
/// <param name="Email">Unique email used as the sign-in identifier.</param>
/// <param name="Password">Plain text password that must be hashed before persistence.</param>
/// <param name="Role">Initial role requested by the registration workflow.</param>
/// <param name="Department">Operational department associated with the user.</param>
/// <param name="Phone">Contact phone provided during registration.</param>
/// <param name="AvatarColor">Frontend avatar fallback color token.</param>
/// <remarks>
///     This command supports TS-12 and returns an authenticated session token after successful
///     registration in the current Sprint 3 scope.
/// </remarks>
public record SignUpCommand(
    string Name,
    string Email,
    string Password,
    string Role,
    string Department,
    string Phone,
    string AvatarColor);

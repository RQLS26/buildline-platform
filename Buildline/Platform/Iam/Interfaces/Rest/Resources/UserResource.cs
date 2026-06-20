namespace Buildline.Platform.Iam.Interfaces.Rest.Resources;

/// <summary>
///     REST resource returned by users-management endpoints.
/// </summary>
/// <param name="Id">User identifier used by API clients.</param>
/// <param name="Name">Display name shown in frontend user views.</param>
/// <param name="Email">Email used as the sign-in identifier.</param>
/// <param name="Role">Application role assigned to the account.</param>
/// <param name="Department">Operational department associated with the account.</param>
/// <param name="Phone">Contact phone shown by the frontend when available.</param>
/// <param name="AvatarColor">Frontend avatar fallback color token.</param>
/// <param name="IsActive">Flag indicating whether the account can authenticate.</param>
/// <param name="LastLogin">Last-login display value expected by the frontend table.</param>
/// <param name="TwoFactorEnabled">Two-factor authentication preference shown in Settings.</param>
public record UserResource(
    int Id,
    string Name,
    string Email,
    string Role,
    string Department,
    string Phone,
    string AvatarColor,
    bool IsActive,
    string LastLogin,
    bool TwoFactorEnabled);

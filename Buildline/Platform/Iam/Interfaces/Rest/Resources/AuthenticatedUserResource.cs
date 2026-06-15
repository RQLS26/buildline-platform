namespace Buildline.Platform.Iam.Interfaces.Rest.Resources;

/// <summary>
///     REST resource returned after successful authentication or registration.
/// </summary>
/// <param name="Id">Authenticated user identifier.</param>
/// <param name="Name">Authenticated user display name.</param>
/// <param name="Email">Authenticated user email.</param>
/// <param name="Role">Authenticated user role claim.</param>
/// <param name="Department">Authenticated user department.</param>
/// <param name="Phone">Authenticated user contact phone.</param>
/// <param name="AvatarColor">Frontend avatar fallback color token.</param>
/// <param name="IsActive">Flag indicating whether the account is active.</param>
/// <param name="LastLogin">Last-login display value.</param>
/// <param name="Token">Signed JWT access token to send in the Authorization header.</param>
public record AuthenticatedUserResource(
    int Id,
    string Name,
    string Email,
    string Role,
    string Department,
    string Phone,
    string AvatarColor,
    bool IsActive,
    string LastLogin,
    string Token);

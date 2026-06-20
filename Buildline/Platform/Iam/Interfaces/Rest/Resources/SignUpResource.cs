namespace Buildline.Platform.Iam.Interfaces.Rest.Resources;

/// <summary>
///     Resource accepted by the sign-up endpoint.
/// </summary>
/// <param name="Name">Display name for the new account.</param>
/// <param name="Email">Email used as the sign-in identifier.</param>
/// <param name="Password">Plain text password that will be hashed by the command service.</param>
/// <param name="Role">Initial role requested for the account.</param>
/// <param name="Department">Operational department associated with the account.</param>
/// <param name="Phone">Contact phone shown in user-management views.</param>
/// <param name="AvatarColor">Frontend avatar fallback color token.</param>
/// <param name="CompanyId">Existing company profile requested by the user, when joining a company.</param>
/// <param name="CompanyName">New company name requested by the user, when creating a company during registration.</param>
public record SignUpResource(
    string Name,
    string Email,
    string Password,
    string Role,
    string Department,
    string Phone,
    string AvatarColor,
    int? CompanyId,
    string? CompanyName);

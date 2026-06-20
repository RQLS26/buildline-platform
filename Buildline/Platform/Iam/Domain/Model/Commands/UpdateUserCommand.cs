namespace Buildline.Platform.Iam.Domain.Model.Commands;

/// <summary>
///     Command that requests a user-management update over an existing IAM account.
/// </summary>
/// <param name="UserId">Identifier of the user aggregate that must be updated.</param>
/// <param name="Name">Display name that should remain visible in user administration screens.</param>
/// <param name="Email">Unique email used as the account sign-in identifier.</param>
/// <param name="Role">Application role assigned to the account.</param>
/// <param name="Department">Operational department associated with the account.</param>
/// <param name="Phone">Contact phone value shown by the frontend when available.</param>
/// <param name="AvatarColor">Frontend avatar fallback color token.</param>
/// <param name="IsActive">Flag that determines whether the user can authenticate.</param>
/// <param name="TwoFactorEnabled">Flag that stores the user's two-factor authentication preference.</param>
/// <remarks>
///     This command belongs to US-024 in Sprint 3. It is intentionally expressed with the full
///     replacement values that the domain aggregate needs; the partial-patch merge happens at the
///     REST assembler boundary, where the current persisted user is available.
/// </remarks>
public record UpdateUserCommand(
    int UserId,
    string Name,
    string Email,
    string Role,
    string Department,
    string Phone,
    string AvatarColor,
    bool IsActive,
    bool TwoFactorEnabled);

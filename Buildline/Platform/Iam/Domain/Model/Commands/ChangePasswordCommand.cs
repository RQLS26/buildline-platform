namespace Buildline.Platform.Iam.Domain.Model.Commands;

/// <summary>
///     Command that requests a credential change for an existing IAM account.
/// </summary>
/// <param name="UserId">Identifier of the account whose password must be changed.</param>
/// <param name="CurrentPassword">Plain current password submitted for verification.</param>
/// <param name="NewPassword">Plain replacement password that will be hashed before persistence.</param>
public record ChangePasswordCommand(int UserId, string CurrentPassword, string NewPassword);

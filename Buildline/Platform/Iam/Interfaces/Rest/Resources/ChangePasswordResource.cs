namespace Buildline.Platform.Iam.Interfaces.Rest.Resources;

/// <summary>Resource accepted by the IAM password-change endpoint.</summary>
/// <param name="CurrentPassword">Current plain password used to verify account ownership.</param>
/// <param name="NewPassword">New plain password that IAM hashes before persistence.</param>
public record ChangePasswordResource(string CurrentPassword, string NewPassword);

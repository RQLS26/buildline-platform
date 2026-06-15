namespace Buildline.Platform.Iam.Domain.Model.Commands;

/// <summary>
///     Command that requests authentication for an existing IAM account.
/// </summary>
/// <param name="Email">Email submitted as the sign-in identifier.</param>
/// <param name="Password">Plain text password submitted for verification.</param>
/// <remarks>
///     This command supports TS-11 and never leaves the application boundary after credential
///     verification completes.
/// </remarks>
public record SignInCommand(string Email, string Password);

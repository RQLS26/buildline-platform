namespace Buildline.Platform.Iam.Domain.Model.Queries;

/// <summary>
///     Query that requests one IAM user by email.
/// </summary>
/// <param name="Email">Email used as the unique user lookup value.</param>
/// <remarks>
///     This query is primarily used by authentication and account uniqueness workflows.
/// </remarks>
public record GetUserByEmailQuery(string Email);

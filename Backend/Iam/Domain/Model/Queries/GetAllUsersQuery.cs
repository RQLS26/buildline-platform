namespace Buildline.Platform.Iam.Domain.Model.Queries;

/// <summary>
///     Query that requests every registered IAM user.
/// </summary>
/// <remarks>
///     This query supports the users-management frontend module and never returns password hashes
///     directly to the REST layer.
/// </remarks>
public record GetAllUsersQuery;

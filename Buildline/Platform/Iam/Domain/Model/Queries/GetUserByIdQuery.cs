namespace Buildline.Platform.Iam.Domain.Model.Queries;

/// <summary>
///     Query that requests one IAM user by identifier.
/// </summary>
/// <param name="UserId">Identifier of the user requested by the API client.</param>
public record GetUserByIdQuery(int UserId);

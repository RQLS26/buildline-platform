using Buildline.Platform.Iam.Application.QueryServices;
using Buildline.Platform.Iam.Domain.Model.Queries;
using Buildline.Platform.Iam.Interfaces.Acl;

namespace Buildline.Platform.Iam.Application.Acl;

/// <summary>
///     Application implementation of the IAM anti-corruption facade.
/// </summary>
/// <param name="userQueryService">IAM query service used to retrieve user identity metadata.</param>
/// <remarks>
///     The facade delegates to the application query service, preserving CQRS boundaries while giving
///     future contexts such as Requisitions, Procurement and Communication a stable identity lookup
///     surface.
/// </remarks>
public class IamContextFacade(IUserQueryService userQueryService) : IIamContextFacade
{
    /// <inheritdoc />
    public async Task<int> FetchUserIdByEmail(string email, CancellationToken cancellationToken = default)
    {
        var user = await userQueryService.Handle(new GetUserByEmailQuery(email), cancellationToken);
        return user?.Id ?? 0;
    }

    /// <inheritdoc />
    public async Task<string> FetchUserEmailById(int userId, CancellationToken cancellationToken = default)
    {
        var user = await userQueryService.Handle(new GetUserByIdQuery(userId), cancellationToken);
        return user?.Email ?? string.Empty;
    }

    /// <inheritdoc />
    public async Task<string> FetchUserRoleById(int userId, CancellationToken cancellationToken = default)
    {
        var user = await userQueryService.Handle(new GetUserByIdQuery(userId), cancellationToken);
        return user?.Role ?? string.Empty;
    }

    /// <inheritdoc />
    public async Task<bool> ExistsUserByEmail(string email, CancellationToken cancellationToken = default)
    {
        var user = await userQueryService.Handle(new GetUserByEmailQuery(email), cancellationToken);
        return user is not null;
    }
}

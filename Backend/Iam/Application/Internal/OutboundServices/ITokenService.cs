using Buildline.Platform.Iam.Domain.Model.Aggregates;

namespace Buildline.Platform.Iam.Application.Internal.OutboundServices;

public interface ITokenService
{
    string GenerateToken(User user);
    Task<int?> ValidateToken(string token);
}

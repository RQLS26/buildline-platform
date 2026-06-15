using Buildline.Platform.Communication.Application.QueryServices;
using Buildline.Platform.Communication.Domain.Model.Aggregates;
using Buildline.Platform.Communication.Domain.Repositories;

namespace Buildline.Platform.Communication.Application.Internal.QueryServices;

/// <summary>Application query service that coordinates inbox reads.</summary>
/// <param name="messageRepository">Repository used to retrieve messages.</param>
public class MessageQueryService(IMessageRepository messageRepository) : IMessageQueryService
{
    /// <inheritdoc />
    public async Task<IEnumerable<Message>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await messageRepository.ListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Message?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await messageRepository.FindByIdAsync(id, cancellationToken);
    }
}

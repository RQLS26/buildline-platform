using Buildline.Platform.Communication.Domain.Model.Aggregates;
using Buildline.Platform.Shared.Domain.Repositories;

namespace Buildline.Platform.Communication.Domain.Repositories;

/// <summary>Repository contract for message aggregate persistence.</summary>
public interface IMessageRepository : IBaseRepository<Message>, ICompanyScopedRepository<Message>;

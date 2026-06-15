using Buildline.Platform.Communication.Domain.Model.Aggregates;
using Buildline.Platform.Communication.Domain.Repositories;
using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Buildline.Platform.Communication.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>Entity Framework Core repository for message aggregates.</summary>
/// <param name="context">Shared application database context.</param>
public class MessageRepository(AppDbContext context) : BaseRepository<Message>(context), IMessageRepository;

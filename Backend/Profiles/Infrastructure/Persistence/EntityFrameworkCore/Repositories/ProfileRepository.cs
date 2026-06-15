using Buildline.Platform.Profiles.Domain.Model.Aggregates;
using Buildline.Platform.Profiles.Domain.Repositories;
using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Buildline.Platform.Profiles.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ProfileRepository(AppDbContext context) : BaseRepository<Profile>(context), IProfileRepository;

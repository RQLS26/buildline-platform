using Buildline.Platform.Iam.Domain.Model.Aggregates;
using Buildline.Platform.Iam.Domain.Repositories;
using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Buildline.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Entity Framework Core repository for IAM user aggregates.
/// </summary>
/// <param name="context">Shared application database context.</param>
/// <remarks>
///     The repository adds email-based queries to the shared repository behavior because email is the
///     unique sign-in identifier in Buildline.
/// </remarks>
public class UserRepository(AppDbContext context) : BaseRepository<User>(context), IUserRepository
{
    /// <inheritdoc />
    public async Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await Context.Set<User>().FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await Context.Set<User>().AnyAsync(user => user.Email == email, cancellationToken);
    }
    /// <inheritdoc />
    public new async Task<IEnumerable<User>> ListByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<User>()
            .Where(user => user.CompanyId == companyId)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public new async Task<User?> FindByIdAndCompanyIdAsync(int id, int companyId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<User>()
            .FirstOrDefaultAsync(user => user.Id == id && user.CompanyId == companyId, cancellationToken);
    }
}

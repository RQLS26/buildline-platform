using Buildline.Platform.Analytics.Application.QueryServices;
using Buildline.Platform.Analytics.Domain.Model.Aggregates;
using Buildline.Platform.Analytics.Domain.Repositories;

namespace Buildline.Platform.Analytics.Application.Internal.QueryServices;

/// <summary>Application query service that coordinates budgeting reads.</summary>
/// <param name="budgetRepository">Repository used to retrieve budgets.</param>
public class BudgetQueryService(IBudgetRepository budgetRepository) : IBudgetQueryService
{
    /// <inheritdoc />
    public async Task<IEnumerable<Budget>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await budgetRepository.ListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Budget?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await budgetRepository.FindByIdAsync(id, cancellationToken);
    }
    /// <inheritdoc />
    public async Task<IEnumerable<Budget>> ListByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default)
    {
        return await budgetRepository.ListByCompanyIdAsync(companyId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Budget?> FindByIdAndCompanyIdAsync(int id, int companyId, CancellationToken cancellationToken = default)
    {
        return await budgetRepository.FindByIdAndCompanyIdAsync(id, companyId, cancellationToken);
    }
}

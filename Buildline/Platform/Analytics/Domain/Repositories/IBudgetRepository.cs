using Buildline.Platform.Analytics.Domain.Model.Aggregates;
using Buildline.Platform.Shared.Domain.Repositories;

namespace Buildline.Platform.Analytics.Domain.Repositories;

/// <summary>Repository contract for budget aggregate persistence.</summary>
public interface IBudgetRepository : IBaseRepository<Budget>;

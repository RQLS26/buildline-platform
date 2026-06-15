using Buildline.Platform.Materials.Application.QueryServices;
using Buildline.Platform.Materials.Domain.Model.Aggregates;
using Buildline.Platform.Materials.Domain.Model.Queries;
using Buildline.Platform.Materials.Domain.Repositories;

namespace Buildline.Platform.Materials.Application.Internal.QueryServices;

public class MaterialQueryService(IMaterialRepository materialRepository) : IMaterialQueryService
{
    public async Task<IEnumerable<Material>> Handle(GetAllMaterialsQuery query, CancellationToken cancellationToken = default)
    {
        return await materialRepository.ListAsync(cancellationToken);
    }

    public async Task<Material?> Handle(GetMaterialByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await materialRepository.FindByIdAsync(query.MaterialId, cancellationToken);
    }
}

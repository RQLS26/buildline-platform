using Buildline.Platform.Materials.Domain.Model.Aggregates;
using Buildline.Platform.Materials.Domain.Model.Commands;
using Buildline.Platform.Shared.Application.Model;

namespace Buildline.Platform.Materials.Application.CommandServices;

public interface IMaterialCommandService
{
    Task<Result<Material>> Handle(CreateMaterialCommand command, CancellationToken cancellationToken = default);
}

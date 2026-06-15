using Buildline.Platform.Materials.Domain.Model.Aggregates;
using Buildline.Platform.Materials.Domain.Model.Commands;
using Buildline.Platform.Shared.Application.Model;

namespace Buildline.Platform.Materials.Application.CommandServices;

public interface IMaterialCommandService
{
    Task<Result<Material>> Handle(CreateMaterialCommand command, CancellationToken cancellationToken = default);
    Task<Result<Material>> Handle(UpdateMaterialCommand command, CancellationToken cancellationToken = default);
    Task<Result> Handle(DeleteMaterialCommand command, CancellationToken cancellationToken = default);
}

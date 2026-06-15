using Buildline.Platform.Materials.Application.CommandServices;
using Buildline.Platform.Materials.Domain.Model;
using Buildline.Platform.Materials.Domain.Model.Aggregates;
using Buildline.Platform.Materials.Domain.Model.Commands;
using Buildline.Platform.Materials.Domain.Repositories;
using Buildline.Platform.Resources.Errors;
using Buildline.Platform.Shared.Application.Model;
using Buildline.Platform.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Buildline.Platform.Materials.Application.Internal.CommandServices;

public class MaterialCommandService(
    IMaterialRepository materialRepository,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ErrorMessages> localizer)
    : IMaterialCommandService
{
    public async Task<Result<Material>> Handle(CreateMaterialCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Name) || string.IsNullOrWhiteSpace(command.Unit))
            return Result<Material>.Failure(
                MaterialsError.InvalidMaterialData,
                localizer[$"{nameof(MaterialsError)}.{MaterialsError.InvalidMaterialData}"]);

        var material = new Material(command);

        try
        {
            await materialRepository.AddAsync(material, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Material>.Success(material);
        }
        catch (OperationCanceledException)
        {
            return Result<Material>.Failure(
                MaterialsError.OperationCancelled,
                localizer[$"{nameof(MaterialsError)}.{MaterialsError.OperationCancelled}"]);
        }
        catch (DbUpdateException)
        {
            return Result<Material>.Failure(
                MaterialsError.DatabaseError,
                localizer[$"{nameof(MaterialsError)}.{MaterialsError.DatabaseError}"]);
        }
        catch (Exception)
        {
            return Result<Material>.Failure(
                MaterialsError.InternalServerError,
                localizer[$"{nameof(MaterialsError)}.{MaterialsError.InternalServerError}"]);
        }
    }
}

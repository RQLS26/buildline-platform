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

    public async Task<Result<Material>> Handle(UpdateMaterialCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Name) || string.IsNullOrWhiteSpace(command.Unit))
            return Result<Material>.Failure(
                MaterialsError.InvalidMaterialData,
                localizer[$"{nameof(MaterialsError)}.{MaterialsError.InvalidMaterialData}"]);

        var material = await materialRepository.FindByIdAsync(command.MaterialId, cancellationToken);
        if (material is null)
            return Result<Material>.Failure(
                MaterialsError.MaterialNotFound,
                localizer[$"{nameof(MaterialsError)}.{MaterialsError.MaterialNotFound}"]);

        try
        {
            material.UpdateCatalogInformation(
                command.Sku,
                command.Name,
                command.Category,
                command.Unit,
                command.Project,
                command.CurrentStock,
                command.MinStock,
                command.MaxStock);
            materialRepository.Update(material);
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

    public async Task<Result> Handle(DeleteMaterialCommand command, CancellationToken cancellationToken = default)
    {
        var material = await materialRepository.FindByIdAsync(command.MaterialId, cancellationToken);
        if (material is null)
            return Result.Failure(
                MaterialsError.MaterialNotFound,
                localizer[$"{nameof(MaterialsError)}.{MaterialsError.MaterialNotFound}"]);

        try
        {
            materialRepository.Remove(material);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result.Success();
        }
        catch (OperationCanceledException)
        {
            return Result.Failure(
                MaterialsError.OperationCancelled,
                localizer[$"{nameof(MaterialsError)}.{MaterialsError.OperationCancelled}"]);
        }
        catch (DbUpdateException)
        {
            return Result.Failure(
                MaterialsError.DatabaseError,
                localizer[$"{nameof(MaterialsError)}.{MaterialsError.DatabaseError}"]);
        }
        catch (Exception)
        {
            return Result.Failure(
                MaterialsError.InternalServerError,
                localizer[$"{nameof(MaterialsError)}.{MaterialsError.InternalServerError}"]);
        }
    }
}

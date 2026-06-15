using Buildline.Platform.Requisition.Application.CommandServices;
using Buildline.Platform.Requisition.Domain.Model;
using Buildline.Platform.Requisition.Domain.Model.Aggregates;
using Buildline.Platform.Requisition.Domain.Model.Commands;
using Buildline.Platform.Requisition.Domain.Repositories;
using Buildline.Platform.Resources.Errors;
using Buildline.Platform.Shared.Application.Model;
using Buildline.Platform.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Buildline.Platform.Requisition.Application.Internal.CommandServices;

/// <summary>
///     Application command service that coordinates material reference writes.
/// </summary>
/// <remarks>
///     The service follows the course layered pattern: REST resources become commands,
///     command handlers validate application rules, aggregates apply state changes, repositories
///     persist them, and <see cref="IUnitOfWork"/> commits the transaction.
/// </remarks>
public class MaterialCommandService(
    IMaterialRepository materialRepository,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ErrorMessages> localizer)
    : IMaterialCommandService
{
    /// <summary>
    ///     Handles material creation for material references.
    /// </summary>
    /// <param name="command">Create-material command with reference and stock fields.</param>
    /// <param name="cancellationToken">Token used to cancel repository and unit-of-work operations.</param>
    /// <returns>
    ///     A successful result with the created material, or a material-domain error for invalid data,
    ///     cancellation, database failure or unexpected server failure.
    /// </returns>
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

    /// <summary>
    ///     Handles replacement of reference and stock information for an existing material.
    /// </summary>
    /// <param name="command">Update command containing the material id and replacement values.</param>
    /// <param name="cancellationToken">Token used to cancel lookup, mutation and persistence.</param>
    /// <returns>
    ///     A successful result with the updated material, or a material-domain error when validation
    ///     fails, the material does not exist, persistence fails, or the operation is cancelled.
    /// </returns>
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
            material.UpdateReferenceInformation(
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

    /// <summary>
    ///     Handles removal of an existing material from material references.
    /// </summary>
    /// <param name="command">Delete command containing the material id.</param>
    /// <param name="cancellationToken">Token used to cancel lookup and persistence.</param>
    /// <returns>
    ///     A successful empty result when the material is removed, or a material-domain error when the
    ///     material does not exist or persistence fails.
    /// </returns>
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



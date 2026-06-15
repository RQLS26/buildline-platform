using Buildline.Platform.Inventory.Application.CommandServices;
using Buildline.Platform.Inventory.Domain.Model;
using Buildline.Platform.Inventory.Domain.Model.Aggregates;
using Buildline.Platform.Inventory.Domain.Model.Commands;
using Buildline.Platform.Inventory.Domain.Repositories;
using Buildline.Platform.Resources.Errors;
using Buildline.Platform.Shared.Application.Model;
using Buildline.Platform.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Buildline.Platform.Inventory.Application.Internal.CommandServices;

/// <summary>
///     Application command service that coordinates inventory item write use cases.
/// </summary>
/// <param name="repository">Repository used to retrieve and persist aggregates.</param>
/// <param name="unitOfWork">Unit of work used to commit aggregate changes transactionally.</param>
/// <param name="localizer">Localizer used to resolve bounded-context error messages.</param>
public class InventoryItemCommandService(
    IInventoryItemRepository repository,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ErrorMessages> localizer)
    : IInventoryItemCommandService
{
    /// <inheritdoc />
    public async Task<Result<InventoryItem>> Handle(CreateInventoryItemCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Name) || string.IsNullOrWhiteSpace(command.Project))
            return Result<InventoryItem>.Failure(
                InventoryError.InvalidInventoryItemData,
                localizer[$"{nameof(InventoryError)}.{InventoryError.InvalidInventoryItemData}"]);

        var aggregate = new InventoryItem(command);

        try
        {
            await repository.AddAsync(aggregate, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<InventoryItem>.Success(aggregate);
        }
        catch (OperationCanceledException)
        {
            return Result<InventoryItem>.Failure(
                InventoryError.OperationCancelled,
                localizer[$"{nameof(InventoryError)}.{InventoryError.OperationCancelled}"]);
        }
        catch (DbUpdateException)
        {
            return Result<InventoryItem>.Failure(
                InventoryError.DatabaseError,
                localizer[$"{nameof(InventoryError)}.{InventoryError.DatabaseError}"]);
        }
        catch (Exception)
        {
            return Result<InventoryItem>.Failure(
                InventoryError.InternalServerError,
                localizer[$"{nameof(InventoryError)}.{InventoryError.InternalServerError}"]);
        }
    }

    /// <inheritdoc />
    public async Task<Result<InventoryItem>> Handle(UpdateInventoryItemCommand command, CancellationToken cancellationToken = default)
    {
        var aggregate = await repository.FindByIdAsync(command.InventoryItemId, cancellationToken);
        if (aggregate is null)
            return Result<InventoryItem>.Failure(
                InventoryError.InventoryItemNotFound,
                localizer[$"{nameof(InventoryError)}.{InventoryError.InventoryItemNotFound}"]);

        try
        {
            aggregate.Apply(command);
            repository.Update(aggregate);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<InventoryItem>.Success(aggregate);
        }
        catch (OperationCanceledException)
        {
            return Result<InventoryItem>.Failure(
                InventoryError.OperationCancelled,
                localizer[$"{nameof(InventoryError)}.{InventoryError.OperationCancelled}"]);
        }
        catch (DbUpdateException)
        {
            return Result<InventoryItem>.Failure(
                InventoryError.DatabaseError,
                localizer[$"{nameof(InventoryError)}.{InventoryError.DatabaseError}"]);
        }
        catch (Exception)
        {
            return Result<InventoryItem>.Failure(
                InventoryError.InternalServerError,
                localizer[$"{nameof(InventoryError)}.{InventoryError.InternalServerError}"]);
        }
    }
}

using Buildline.Platform.Procurement.Application.CommandServices;
using Buildline.Platform.Procurement.Domain.Model;
using Buildline.Platform.Procurement.Domain.Model.Aggregates;
using Buildline.Platform.Procurement.Domain.Model.Commands;
using Buildline.Platform.Procurement.Domain.Repositories;
using Buildline.Platform.Resources.Errors;
using Buildline.Platform.Shared.Application.Model;
using Buildline.Platform.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Buildline.Platform.Procurement.Application.Internal.CommandServices;

/// <summary>
///     Application command service that coordinates purchase order write use cases.
/// </summary>
/// <param name="repository">Repository used to retrieve and persist aggregates.</param>
/// <param name="unitOfWork">Unit of work used to commit aggregate changes transactionally.</param>
/// <param name="localizer">Localizer used to resolve bounded-context error messages.</param>
public class PurchaseOrderCommandService(
    IPurchaseOrderRepository repository,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ErrorMessages> localizer)
    : IPurchaseOrderCommandService
{
    /// <inheritdoc />
    public async Task<Result<PurchaseOrder>> Handle(CreatePurchaseOrderCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.SupplierName) || string.IsNullOrWhiteSpace(command.Material))
            return Result<PurchaseOrder>.Failure(
                ProcurementError.InvalidPurchaseOrderData,
                localizer[$"{nameof(ProcurementError)}.{ProcurementError.InvalidPurchaseOrderData}"]);

        var aggregate = new PurchaseOrder(command);

        try
        {
            await repository.AddAsync(aggregate, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<PurchaseOrder>.Success(aggregate);
        }
        catch (OperationCanceledException)
        {
            return Result<PurchaseOrder>.Failure(
                ProcurementError.OperationCancelled,
                localizer[$"{nameof(ProcurementError)}.{ProcurementError.OperationCancelled}"]);
        }
        catch (DbUpdateException)
        {
            return Result<PurchaseOrder>.Failure(
                ProcurementError.DatabaseError,
                localizer[$"{nameof(ProcurementError)}.{ProcurementError.DatabaseError}"]);
        }
        catch (Exception)
        {
            return Result<PurchaseOrder>.Failure(
                ProcurementError.InternalServerError,
                localizer[$"{nameof(ProcurementError)}.{ProcurementError.InternalServerError}"]);
        }
    }

    /// <inheritdoc />
    public async Task<Result<PurchaseOrder>> Handle(UpdatePurchaseOrderCommand command, CancellationToken cancellationToken = default)
    {
        var aggregate = await repository.FindByIdAsync(command.PurchaseOrderId, cancellationToken);
        if (aggregate is null)
            return Result<PurchaseOrder>.Failure(
                ProcurementError.PurchaseOrderNotFound,
                localizer[$"{nameof(ProcurementError)}.{ProcurementError.PurchaseOrderNotFound}"]);

        try
        {
            aggregate.Apply(command);
            repository.Update(aggregate);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<PurchaseOrder>.Success(aggregate);
        }
        catch (OperationCanceledException)
        {
            return Result<PurchaseOrder>.Failure(
                ProcurementError.OperationCancelled,
                localizer[$"{nameof(ProcurementError)}.{ProcurementError.OperationCancelled}"]);
        }
        catch (DbUpdateException)
        {
            return Result<PurchaseOrder>.Failure(
                ProcurementError.DatabaseError,
                localizer[$"{nameof(ProcurementError)}.{ProcurementError.DatabaseError}"]);
        }
        catch (Exception)
        {
            return Result<PurchaseOrder>.Failure(
                ProcurementError.InternalServerError,
                localizer[$"{nameof(ProcurementError)}.{ProcurementError.InternalServerError}"]);
        }
    }
}

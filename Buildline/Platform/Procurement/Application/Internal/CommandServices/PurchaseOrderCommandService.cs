using Buildline.Platform.Procurement.Application.CommandServices;
using Buildline.Platform.Procurement.Application.Internal.OutboundServices;
using Buildline.Platform.Procurement.Domain.Model;
using Buildline.Platform.Procurement.Domain.Model.Aggregates;
using Buildline.Platform.Procurement.Domain.Model.Commands;
using Buildline.Platform.Procurement.Domain.Repositories;
using Buildline.Platform.Procurement.Resources;
using Buildline.Platform.Shared.Application.Model;
using Buildline.Platform.Shared.Domain.Model.Entities;
using Buildline.Platform.Shared.Domain.Repositories;
using Cortex.Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Buildline.Platform.Procurement.Application.Internal.CommandServices;

/// <summary>
///     Application command service that coordinates purchase order write use cases.
/// </summary>
/// <param name="repository">Repository used to retrieve and persist purchase order aggregates.</param>
/// <param name="supplierDirectoryService">Outbound service used to validate supplier references owned by Suppliers.</param>
/// <param name="unitOfWork">Unit of work used to commit aggregate changes transactionally.</param>
/// <param name="mediator">Cortex mediator used to publish domain events after aggregate persistence.</param>
/// <param name="localizer">Localizer used to resolve bounded-context error messages.</param>
/// <remarks>
///     The service centralizes purchase order invariants that depend on external context data, keeping the
///     aggregate focused on its own state and keeping controller actions free from persistence orchestration.
/// </remarks>
public class PurchaseOrderCommandService(
    IPurchaseOrderRepository repository,
    ISupplierDirectoryService supplierDirectoryService,
    IUnitOfWork unitOfWork,
    IMediator mediator,
    IStringLocalizer<ProcurementMessages> localizer)
    : IPurchaseOrderCommandService
{
    /// <inheritdoc />
    public async Task<Result<PurchaseOrder>> Handle(CreatePurchaseOrderCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.SupplierName) || string.IsNullOrWhiteSpace(command.Material))
            return Result<PurchaseOrder>.Failure(
                ProcurementError.InvalidPurchaseOrderData,
                localizer[$"{nameof(ProcurementError)}.{ProcurementError.InvalidPurchaseOrderData}"]);

        if (!await supplierDirectoryService.SupplierCanReceiveOrdersForAsync(command, cancellationToken))
            return Result<PurchaseOrder>.Failure(
                ProcurementError.SupplierReferenceNotSelectable,
                localizer[$"{nameof(ProcurementError)}.{ProcurementError.SupplierReferenceNotSelectable}"]);

        var aggregate = new PurchaseOrder(command);

        try
        {
            await repository.AddAsync(aggregate, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            await PublishDomainEventsAsync(aggregate, cancellationToken);
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
            await PublishDomainEventsAsync(aggregate, cancellationToken);
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

    private async Task PublishDomainEventsAsync(PurchaseOrder aggregate, CancellationToken cancellationToken)
    {
        if (aggregate is IHasDomainEvents hasEvents && hasEvents.DomainEvents.Count != 0)
        {
            foreach (var domainEvent in hasEvents.DomainEvents)
                await mediator.PublishAsync(domainEvent, cancellationToken);
            hasEvents.ClearDomainEvents();
        }
    }
}

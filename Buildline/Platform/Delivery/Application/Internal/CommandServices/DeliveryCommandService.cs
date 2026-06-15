using Buildline.Platform.Delivery.Application.CommandServices;
using Buildline.Platform.Delivery.Domain.Model;
using DeliveryAggregate = Buildline.Platform.Delivery.Domain.Model.Aggregates.Delivery;
using Buildline.Platform.Delivery.Domain.Model.Commands;
using Buildline.Platform.Delivery.Domain.Repositories;
using Buildline.Platform.Resources.Errors;
using Buildline.Platform.Shared.Application.Model;
using Buildline.Platform.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Buildline.Platform.Delivery.Application.Internal.CommandServices;

/// <summary>
///     Application command service that coordinates delivery write use cases.
/// </summary>
/// <param name="repository">Repository used to retrieve and persist aggregates.</param>
/// <param name="unitOfWork">Unit of work used to commit aggregate changes transactionally.</param>
/// <param name="localizer">Localizer used to resolve bounded-context error messages.</param>
public class DeliveryCommandService(
    IDeliveryRepository repository,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ErrorMessages> localizer)
    : IDeliveryCommandService
{
    /// <inheritdoc />
    public async Task<Result<DeliveryAggregate>> Handle(CreateDeliveryCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.PurchaseOrder) || string.IsNullOrWhiteSpace(command.Supplier))
            return Result<DeliveryAggregate>.Failure(
                DeliveryError.InvalidDeliveryData,
                localizer[$"{nameof(DeliveryError)}.{DeliveryError.InvalidDeliveryData}"]);

        var aggregate = new DeliveryAggregate(command);

        try
        {
            await repository.AddAsync(aggregate, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<DeliveryAggregate>.Success(aggregate);
        }
        catch (OperationCanceledException)
        {
            return Result<DeliveryAggregate>.Failure(
                DeliveryError.OperationCancelled,
                localizer[$"{nameof(DeliveryError)}.{DeliveryError.OperationCancelled}"]);
        }
        catch (DbUpdateException)
        {
            return Result<DeliveryAggregate>.Failure(
                DeliveryError.DatabaseError,
                localizer[$"{nameof(DeliveryError)}.{DeliveryError.DatabaseError}"]);
        }
        catch (Exception)
        {
            return Result<DeliveryAggregate>.Failure(
                DeliveryError.InternalServerError,
                localizer[$"{nameof(DeliveryError)}.{DeliveryError.InternalServerError}"]);
        }
    }

    /// <inheritdoc />
    public async Task<Result<DeliveryAggregate>> Handle(UpdateDeliveryCommand command, CancellationToken cancellationToken = default)
    {
        var aggregate = await repository.FindByIdAsync(command.DeliveryId, cancellationToken);
        if (aggregate is null)
            return Result<DeliveryAggregate>.Failure(
                DeliveryError.DeliveryNotFound,
                localizer[$"{nameof(DeliveryError)}.{DeliveryError.DeliveryNotFound}"]);

        try
        {
            aggregate.Apply(command);
            repository.Update(aggregate);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<DeliveryAggregate>.Success(aggregate);
        }
        catch (OperationCanceledException)
        {
            return Result<DeliveryAggregate>.Failure(
                DeliveryError.OperationCancelled,
                localizer[$"{nameof(DeliveryError)}.{DeliveryError.OperationCancelled}"]);
        }
        catch (DbUpdateException)
        {
            return Result<DeliveryAggregate>.Failure(
                DeliveryError.DatabaseError,
                localizer[$"{nameof(DeliveryError)}.{DeliveryError.DatabaseError}"]);
        }
        catch (Exception)
        {
            return Result<DeliveryAggregate>.Failure(
                DeliveryError.InternalServerError,
                localizer[$"{nameof(DeliveryError)}.{DeliveryError.InternalServerError}"]);
        }
    }
}


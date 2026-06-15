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
///     Application command service that coordinates quotation write use cases.
/// </summary>
/// <param name="repository">Repository used to retrieve and persist aggregates.</param>
/// <param name="unitOfWork">Unit of work used to commit aggregate changes transactionally.</param>
/// <param name="localizer">Localizer used to resolve bounded-context error messages.</param>
public class QuotationCommandService(
    IQuotationRepository repository,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ErrorMessages> localizer)
    : IQuotationCommandService
{
    /// <inheritdoc />
    public async Task<Result<Quotation>> Handle(CreateQuotationCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Supplier) || string.IsNullOrWhiteSpace(command.Material))
            return Result<Quotation>.Failure(
                ProcurementError.InvalidQuotationData,
                localizer[$"{nameof(ProcurementError)}.{ProcurementError.InvalidQuotationData}"]);

        var aggregate = new Quotation(command);

        try
        {
            await repository.AddAsync(aggregate, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Quotation>.Success(aggregate);
        }
        catch (OperationCanceledException)
        {
            return Result<Quotation>.Failure(
                ProcurementError.OperationCancelled,
                localizer[$"{nameof(ProcurementError)}.{ProcurementError.OperationCancelled}"]);
        }
        catch (DbUpdateException)
        {
            return Result<Quotation>.Failure(
                ProcurementError.DatabaseError,
                localizer[$"{nameof(ProcurementError)}.{ProcurementError.DatabaseError}"]);
        }
        catch (Exception)
        {
            return Result<Quotation>.Failure(
                ProcurementError.InternalServerError,
                localizer[$"{nameof(ProcurementError)}.{ProcurementError.InternalServerError}"]);
        }
    }

    /// <inheritdoc />
    public async Task<Result<Quotation>> Handle(UpdateQuotationCommand command, CancellationToken cancellationToken = default)
    {
        var aggregate = await repository.FindByIdAsync(command.QuotationPersistenceId, cancellationToken);
        if (aggregate is null)
            return Result<Quotation>.Failure(
                ProcurementError.QuotationNotFound,
                localizer[$"{nameof(ProcurementError)}.{ProcurementError.QuotationNotFound}"]);

        try
        {
            aggregate.Apply(command);
            repository.Update(aggregate);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Quotation>.Success(aggregate);
        }
        catch (OperationCanceledException)
        {
            return Result<Quotation>.Failure(
                ProcurementError.OperationCancelled,
                localizer[$"{nameof(ProcurementError)}.{ProcurementError.OperationCancelled}"]);
        }
        catch (DbUpdateException)
        {
            return Result<Quotation>.Failure(
                ProcurementError.DatabaseError,
                localizer[$"{nameof(ProcurementError)}.{ProcurementError.DatabaseError}"]);
        }
        catch (Exception)
        {
            return Result<Quotation>.Failure(
                ProcurementError.InternalServerError,
                localizer[$"{nameof(ProcurementError)}.{ProcurementError.InternalServerError}"]);
        }
    }
}


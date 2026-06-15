using Buildline.Platform.Suppliers.Application.CommandServices;
using Buildline.Platform.Suppliers.Domain.Model;
using Buildline.Platform.Suppliers.Domain.Model.Aggregates;
using Buildline.Platform.Suppliers.Domain.Model.Commands;
using Buildline.Platform.Suppliers.Domain.Repositories;
using Buildline.Platform.Resources.Errors;
using Buildline.Platform.Shared.Application.Model;
using Buildline.Platform.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Buildline.Platform.Suppliers.Application.Internal.CommandServices;

/// <summary>
///     Application command service that coordinates incident write use cases.
/// </summary>
/// <param name="repository">Repository used to retrieve and persist aggregates.</param>
/// <param name="unitOfWork">Unit of work used to commit aggregate changes transactionally.</param>
/// <param name="localizer">Localizer used to resolve bounded-context error messages.</param>
public class SupplierIncidentCommandService(
    ISupplierIncidentRepository repository,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ErrorMessages> localizer)
    : ISupplierIncidentCommandService
{
    /// <inheritdoc />
    public async Task<Result<SupplierIncident>> Handle(CreateSupplierIncidentCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Title))
            return Result<SupplierIncident>.Failure(
                SuppliersError.InvalidSupplierIncidentData,
                localizer[$"{nameof(SuppliersError)}.{SuppliersError.InvalidSupplierIncidentData}"]);

        var aggregate = new SupplierIncident(command);

        try
        {
            await repository.AddAsync(aggregate, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<SupplierIncident>.Success(aggregate);
        }
        catch (OperationCanceledException)
        {
            return Result<SupplierIncident>.Failure(
                SuppliersError.OperationCancelled,
                localizer[$"{nameof(SuppliersError)}.{SuppliersError.OperationCancelled}"]);
        }
        catch (DbUpdateException)
        {
            return Result<SupplierIncident>.Failure(
                SuppliersError.DatabaseError,
                localizer[$"{nameof(SuppliersError)}.{SuppliersError.DatabaseError}"]);
        }
        catch (Exception)
        {
            return Result<SupplierIncident>.Failure(
                SuppliersError.InternalServerError,
                localizer[$"{nameof(SuppliersError)}.{SuppliersError.InternalServerError}"]);
        }
    }

    /// <inheritdoc />
    public async Task<Result<SupplierIncident>> Handle(UpdateSupplierIncidentCommand command, CancellationToken cancellationToken = default)
    {
        var aggregate = await repository.FindByIdAsync(command.SupplierIncidentId, cancellationToken);
        if (aggregate is null)
            return Result<SupplierIncident>.Failure(
                SuppliersError.SupplierIncidentNotFound,
                localizer[$"{nameof(SuppliersError)}.{SuppliersError.SupplierIncidentNotFound}"]);

        try
        {
            aggregate.Apply(command);
            repository.Update(aggregate);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<SupplierIncident>.Success(aggregate);
        }
        catch (OperationCanceledException)
        {
            return Result<SupplierIncident>.Failure(
                SuppliersError.OperationCancelled,
                localizer[$"{nameof(SuppliersError)}.{SuppliersError.OperationCancelled}"]);
        }
        catch (DbUpdateException)
        {
            return Result<SupplierIncident>.Failure(
                SuppliersError.DatabaseError,
                localizer[$"{nameof(SuppliersError)}.{SuppliersError.DatabaseError}"]);
        }
        catch (Exception)
        {
            return Result<SupplierIncident>.Failure(
                SuppliersError.InternalServerError,
                localizer[$"{nameof(SuppliersError)}.{SuppliersError.InternalServerError}"]);
        }
    }
}

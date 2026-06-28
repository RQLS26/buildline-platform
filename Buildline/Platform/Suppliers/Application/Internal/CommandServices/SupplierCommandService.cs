using Buildline.Platform.Suppliers.Application.CommandServices;
using Buildline.Platform.Suppliers.Domain.Model;
using Buildline.Platform.Suppliers.Domain.Model.Aggregates;
using Buildline.Platform.Suppliers.Domain.Model.Commands;
using Buildline.Platform.Suppliers.Domain.Repositories;
using Buildline.Platform.Suppliers.Resources;
using Buildline.Platform.Shared.Application.Model;
using Buildline.Platform.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Buildline.Platform.Suppliers.Application.Internal.CommandServices;

/// <summary>
///     Application command service that coordinates supplier write use cases.
/// </summary>
/// <param name="repository">Repository used to retrieve and persist aggregates.</param>
/// <param name="unitOfWork">Unit of work used to commit aggregate changes transactionally.</param>
/// <param name="localizer">Localizer used to resolve bounded-context error messages.</param>
public class SupplierCommandService(
    ISupplierRepository repository,
    IUnitOfWork unitOfWork,
    IStringLocalizer<SuppliersMessages> localizer)
    : ISupplierCommandService
{
    /// <inheritdoc />
    public async Task<Result<Supplier>> Handle(CreateSupplierCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.CompanyName) || string.IsNullOrWhiteSpace(command.Ruc))
            return Result<Supplier>.Failure(
                SuppliersError.InvalidSupplierData,
                localizer[$"{nameof(SuppliersError)}.{SuppliersError.InvalidSupplierData}"]);

        var aggregate = new Supplier(command);

        try
        {
            await repository.AddAsync(aggregate, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Supplier>.Success(aggregate);
        }
        catch (OperationCanceledException)
        {
            return Result<Supplier>.Failure(
                SuppliersError.OperationCancelled,
                localizer[$"{nameof(SuppliersError)}.{SuppliersError.OperationCancelled}"]);
        }
        catch (DbUpdateException)
        {
            return Result<Supplier>.Failure(
                SuppliersError.DatabaseError,
                localizer[$"{nameof(SuppliersError)}.{SuppliersError.DatabaseError}"]);
        }
        catch (Exception)
        {
            return Result<Supplier>.Failure(
                SuppliersError.InternalServerError,
                localizer[$"{nameof(SuppliersError)}.{SuppliersError.InternalServerError}"]);
        }
    }

    /// <inheritdoc />
    public async Task<Result<Supplier>> Handle(UpdateSupplierCommand command, CancellationToken cancellationToken = default)
    {
        var aggregate = await repository.FindByIdAsync(command.SupplierId, cancellationToken);
        if (aggregate is null)
            return Result<Supplier>.Failure(
                SuppliersError.SupplierNotFound,
                localizer[$"{nameof(SuppliersError)}.{SuppliersError.SupplierNotFound}"]);

        try
        {
            aggregate.Apply(command);
            repository.Update(aggregate);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Supplier>.Success(aggregate);
        }
        catch (OperationCanceledException)
        {
            return Result<Supplier>.Failure(
                SuppliersError.OperationCancelled,
                localizer[$"{nameof(SuppliersError)}.{SuppliersError.OperationCancelled}"]);
        }
        catch (DbUpdateException)
        {
            return Result<Supplier>.Failure(
                SuppliersError.DatabaseError,
                localizer[$"{nameof(SuppliersError)}.{SuppliersError.DatabaseError}"]);
        }
        catch (Exception)
        {
            return Result<Supplier>.Failure(
                SuppliersError.InternalServerError,
                localizer[$"{nameof(SuppliersError)}.{SuppliersError.InternalServerError}"]);
        }
    }
    /// <inheritdoc />
    public async Task<Result> HandleDelete(int supplierId, CancellationToken cancellationToken = default)
    {
        var aggregate = await repository.FindByIdAsync(supplierId, cancellationToken);
        if (aggregate is null)
            return Result.Failure(
                SuppliersError.SupplierNotFound,
                localizer[$"{nameof(SuppliersError)}.{SuppliersError.SupplierNotFound}"]);

        try
        {
            repository.Remove(aggregate);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result.Success();
        }
        catch (OperationCanceledException)
        {
            return Result.Failure(
                SuppliersError.OperationCancelled,
                localizer[$"{nameof(SuppliersError)}.{SuppliersError.OperationCancelled}"]);
        }
        catch (DbUpdateException)
        {
            return Result.Failure(
                SuppliersError.DatabaseError,
                localizer[$"{nameof(SuppliersError)}.{SuppliersError.DatabaseError}"]);
        }
        catch (Exception)
        {
            return Result.Failure(
                SuppliersError.InternalServerError,
                localizer[$"{nameof(SuppliersError)}.{SuppliersError.InternalServerError}"]);
        }
    }
}

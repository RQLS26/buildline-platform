using Buildline.Platform.Requisition.Application.CommandServices;
using Buildline.Platform.Requisition.Application.Internal.OutboundServices;
using Buildline.Platform.Requisition.Domain.Model;
using RequisitionAggregate = Buildline.Platform.Requisition.Domain.Model.Aggregates.Requisition;
using Buildline.Platform.Requisition.Domain.Model.Commands;
using Buildline.Platform.Requisition.Domain.Repositories;
using Buildline.Platform.Resources.Errors;
using Buildline.Platform.Shared.Application.Model;
using Buildline.Platform.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Buildline.Platform.Requisition.Application.Internal.CommandServices;

/// <summary>
///     Application command service that coordinates requisition write use cases.
/// </summary>
/// <param name="repository">Repository used to retrieve and persist requisition aggregates.</param>
/// <param name="projectReferenceService">Outbound service used to validate project references owned by Analytics.</param>
/// <param name="unitOfWork">Unit of work used to commit aggregate changes transactionally.</param>
/// <param name="localizer">Localizer used to resolve bounded-context error messages.</param>
/// <remarks>
///     The service keeps the controller thin, validates frontend payloads, checks cross-context references
///     through an outbound service and returns a typed <see cref="Result{T}" /> so REST assemblers can map
///     failures consistently to Problem Details responses.
/// </remarks>
public class RequisitionCommandService(
    IRequisitionRepository repository,
    IProjectReferenceService projectReferenceService,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ErrorMessages> localizer)
    : IRequisitionCommandService
{
    /// <inheritdoc />
    public async Task<Result<RequisitionAggregate>> Handle(CreateRequisitionCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Material) || string.IsNullOrWhiteSpace(command.Project))
            return Result<RequisitionAggregate>.Failure(
                RequisitionError.InvalidRequisitionData,
                localizer[$"{nameof(RequisitionError)}.{RequisitionError.InvalidRequisitionData}"]);

        if (!await projectReferenceService.ProjectExistsForAsync(command, cancellationToken))
            return Result<RequisitionAggregate>.Failure(
                RequisitionError.ProjectReferenceNotFound,
                localizer[$"{nameof(RequisitionError)}.{RequisitionError.ProjectReferenceNotFound}"]);

        var aggregate = new RequisitionAggregate(command);

        try
        {
            await repository.AddAsync(aggregate, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<RequisitionAggregate>.Success(aggregate);
        }
        catch (OperationCanceledException)
        {
            return Result<RequisitionAggregate>.Failure(
                RequisitionError.OperationCancelled,
                localizer[$"{nameof(RequisitionError)}.{RequisitionError.OperationCancelled}"]);
        }
        catch (DbUpdateException)
        {
            return Result<RequisitionAggregate>.Failure(
                RequisitionError.DatabaseError,
                localizer[$"{nameof(RequisitionError)}.{RequisitionError.DatabaseError}"]);
        }
        catch (Exception)
        {
            return Result<RequisitionAggregate>.Failure(
                RequisitionError.InternalServerError,
                localizer[$"{nameof(RequisitionError)}.{RequisitionError.InternalServerError}"]);
        }
    }

    /// <inheritdoc />
    public async Task<Result<RequisitionAggregate>> Handle(UpdateRequisitionCommand command, CancellationToken cancellationToken = default)
    {
        var aggregate = await repository.FindByIdAsync(command.RequisitionId, cancellationToken);
        if (aggregate is null)
            return Result<RequisitionAggregate>.Failure(
                RequisitionError.RequisitionNotFound,
                localizer[$"{nameof(RequisitionError)}.{RequisitionError.RequisitionNotFound}"]);

        try
        {
            aggregate.Apply(command);
            repository.Update(aggregate);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<RequisitionAggregate>.Success(aggregate);
        }
        catch (OperationCanceledException)
        {
            return Result<RequisitionAggregate>.Failure(
                RequisitionError.OperationCancelled,
                localizer[$"{nameof(RequisitionError)}.{RequisitionError.OperationCancelled}"]);
        }
        catch (DbUpdateException)
        {
            return Result<RequisitionAggregate>.Failure(
                RequisitionError.DatabaseError,
                localizer[$"{nameof(RequisitionError)}.{RequisitionError.DatabaseError}"]);
        }
        catch (Exception)
        {
            return Result<RequisitionAggregate>.Failure(
                RequisitionError.InternalServerError,
                localizer[$"{nameof(RequisitionError)}.{RequisitionError.InternalServerError}"]);
        }
    }
}

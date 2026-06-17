using Buildline.Platform.Analytics.Application.CommandServices;
using Buildline.Platform.Analytics.Domain.Model;
using Buildline.Platform.Analytics.Domain.Model.Aggregates;
using Buildline.Platform.Analytics.Domain.Model.Commands;
using Buildline.Platform.Analytics.Domain.Repositories;
using Buildline.Platform.Resources.Errors;
using Buildline.Platform.Shared.Application.Model;
using Buildline.Platform.Shared.Domain.Model.Entities;
using Buildline.Platform.Shared.Domain.Repositories;
using Cortex.Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Buildline.Platform.Analytics.Application.Internal.CommandServices;

/// <summary>
///     Application command service that coordinates budget write use cases.
/// </summary>
/// <param name="repository">Repository used to retrieve and persist aggregates.</param>
/// <param name="unitOfWork">Unit of work used to commit aggregate changes transactionally.</param>
/// <param name="localizer">Localizer used to resolve bounded-context error messages.</param>
public class BudgetCommandService(
    IBudgetRepository repository,
    IUnitOfWork unitOfWork,
    IMediator mediator,
    IStringLocalizer<ErrorMessages> localizer)
    : IBudgetCommandService
{
    /// <inheritdoc />
    public async Task<Result<Budget>> Handle(CreateBudgetCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Project))
            return Result<Budget>.Failure(
                AnalyticsError.InvalidBudgetData,
                localizer[$"{nameof(AnalyticsError)}.{AnalyticsError.InvalidBudgetData}"]);

        var aggregate = new Budget(command);

        try
        {
            await repository.AddAsync(aggregate, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            await PublishDomainEventsAsync(aggregate, cancellationToken);
            return Result<Budget>.Success(aggregate);
        }
        catch (OperationCanceledException)
        {
            return Result<Budget>.Failure(
                AnalyticsError.OperationCancelled,
                localizer[$"{nameof(AnalyticsError)}.{AnalyticsError.OperationCancelled}"]);
        }
        catch (DbUpdateException)
        {
            return Result<Budget>.Failure(
                AnalyticsError.DatabaseError,
                localizer[$"{nameof(AnalyticsError)}.{AnalyticsError.DatabaseError}"]);
        }
        catch (Exception)
        {
            return Result<Budget>.Failure(
                AnalyticsError.InternalServerError,
                localizer[$"{nameof(AnalyticsError)}.{AnalyticsError.InternalServerError}"]);
        }
    }

    /// <inheritdoc />
    public async Task<Result<Budget>> Handle(UpdateBudgetCommand command, CancellationToken cancellationToken = default)
    {
        var aggregate = await repository.FindByIdAsync(command.BudgetId, cancellationToken);
        if (aggregate is null)
            return Result<Budget>.Failure(
                AnalyticsError.BudgetNotFound,
                localizer[$"{nameof(AnalyticsError)}.{AnalyticsError.BudgetNotFound}"]);

        try
        {
            aggregate.Apply(command);
            repository.Update(aggregate);
            await unitOfWork.CompleteAsync(cancellationToken);
            await PublishDomainEventsAsync(aggregate, cancellationToken);
            return Result<Budget>.Success(aggregate);
        }
        catch (OperationCanceledException)
        {
            return Result<Budget>.Failure(
                AnalyticsError.OperationCancelled,
                localizer[$"{nameof(AnalyticsError)}.{AnalyticsError.OperationCancelled}"]);
        }
        catch (DbUpdateException)
        {
            return Result<Budget>.Failure(
                AnalyticsError.DatabaseError,
                localizer[$"{nameof(AnalyticsError)}.{AnalyticsError.DatabaseError}"]);
        }
        catch (Exception)
        {
            return Result<Budget>.Failure(
                AnalyticsError.InternalServerError,
                localizer[$"{nameof(AnalyticsError)}.{AnalyticsError.InternalServerError}"]);
        }
    }

    private async Task PublishDomainEventsAsync(Budget aggregate, CancellationToken cancellationToken)
    {
        if (aggregate is IHasDomainEvents hasEvents && hasEvents.DomainEvents.Count != 0)
        {
            foreach (var domainEvent in hasEvents.DomainEvents)
                await mediator.PublishAsync(domainEvent, cancellationToken);
            hasEvents.ClearDomainEvents();
        }
    }
}

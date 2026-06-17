using Buildline.Platform.Communication.Application.CommandServices;
using Buildline.Platform.Communication.Domain.Model;
using Buildline.Platform.Communication.Domain.Model.Aggregates;
using Buildline.Platform.Communication.Domain.Model.Commands;
using Buildline.Platform.Communication.Domain.Repositories;
using Buildline.Platform.Resources.Errors;
using Buildline.Platform.Shared.Application.Model;
using Buildline.Platform.Shared.Domain.Model.Entities;
using Buildline.Platform.Shared.Domain.Repositories;
using Cortex.Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Buildline.Platform.Communication.Application.Internal.CommandServices;

/// <summary>
///     Application command service that coordinates message write use cases.
/// </summary>
/// <param name="repository">Repository used to retrieve and persist aggregates.</param>
/// <param name="unitOfWork">Unit of work used to commit aggregate changes transactionally.</param>
/// <param name="localizer">Localizer used to resolve bounded-context error messages.</param>
public class MessageCommandService(
    IMessageRepository repository,
    IUnitOfWork unitOfWork,
    IMediator mediator,
    IStringLocalizer<ErrorMessages> localizer)
    : IMessageCommandService
{
    /// <inheritdoc />
    public async Task<Result<Message>> Handle(CreateMessageCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Subject))
            return Result<Message>.Failure(
                CommunicationError.InvalidMessageData,
                localizer[$"{nameof(CommunicationError)}.{CommunicationError.InvalidMessageData}"]);

        var aggregate = new Message(command);

        try
        {
            await repository.AddAsync(aggregate, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            await PublishDomainEventsAsync(aggregate, cancellationToken);
            return Result<Message>.Success(aggregate);
        }
        catch (OperationCanceledException)
        {
            return Result<Message>.Failure(
                CommunicationError.OperationCancelled,
                localizer[$"{nameof(CommunicationError)}.{CommunicationError.OperationCancelled}"]);
        }
        catch (DbUpdateException)
        {
            return Result<Message>.Failure(
                CommunicationError.DatabaseError,
                localizer[$"{nameof(CommunicationError)}.{CommunicationError.DatabaseError}"]);
        }
        catch (Exception)
        {
            return Result<Message>.Failure(
                CommunicationError.InternalServerError,
                localizer[$"{nameof(CommunicationError)}.{CommunicationError.InternalServerError}"]);
        }
    }

    /// <inheritdoc />
    public async Task<Result<Message>> Handle(UpdateMessageCommand command, CancellationToken cancellationToken = default)
    {
        var aggregate = await repository.FindByIdAsync(command.MessageId, cancellationToken);
        if (aggregate is null)
            return Result<Message>.Failure(
                CommunicationError.MessageNotFound,
                localizer[$"{nameof(CommunicationError)}.{CommunicationError.MessageNotFound}"]);

        try
        {
            aggregate.Apply(command);
            repository.Update(aggregate);
            await unitOfWork.CompleteAsync(cancellationToken);
            await PublishDomainEventsAsync(aggregate, cancellationToken);
            return Result<Message>.Success(aggregate);
        }
        catch (OperationCanceledException)
        {
            return Result<Message>.Failure(
                CommunicationError.OperationCancelled,
                localizer[$"{nameof(CommunicationError)}.{CommunicationError.OperationCancelled}"]);
        }
        catch (DbUpdateException)
        {
            return Result<Message>.Failure(
                CommunicationError.DatabaseError,
                localizer[$"{nameof(CommunicationError)}.{CommunicationError.DatabaseError}"]);
        }
        catch (Exception)
        {
            return Result<Message>.Failure(
                CommunicationError.InternalServerError,
                localizer[$"{nameof(CommunicationError)}.{CommunicationError.InternalServerError}"]);
        }
    }
    /// <inheritdoc />
    public async Task<Result> HandleDelete(int messageId, CancellationToken cancellationToken = default)
    {
        var aggregate = await repository.FindByIdAsync(messageId, cancellationToken);
        if (aggregate is null)
            return Result.Failure(
                CommunicationError.MessageNotFound,
                localizer[$"{nameof(CommunicationError)}.{CommunicationError.MessageNotFound}"]);

        try
        {
            repository.Remove(aggregate);
            await unitOfWork.CompleteAsync(cancellationToken);
            await PublishDomainEventsAsync(aggregate, cancellationToken);
            return Result.Success();
        }
        catch (OperationCanceledException)
        {
            return Result.Failure(
                CommunicationError.OperationCancelled,
                localizer[$"{nameof(CommunicationError)}.{CommunicationError.OperationCancelled}"]);
        }
        catch (DbUpdateException)
        {
            return Result.Failure(
                CommunicationError.DatabaseError,
                localizer[$"{nameof(CommunicationError)}.{CommunicationError.DatabaseError}"]);
        }
        catch (Exception)
        {
            return Result.Failure(
                CommunicationError.InternalServerError,
                localizer[$"{nameof(CommunicationError)}.{CommunicationError.InternalServerError}"]);
        }
    }

    private async Task PublishDomainEventsAsync(Message aggregate, CancellationToken cancellationToken)
    {
        if (aggregate is IHasDomainEvents hasEvents && hasEvents.DomainEvents.Count != 0)
        {
            foreach (var domainEvent in hasEvents.DomainEvents)
                await mediator.PublishAsync(domainEvent, cancellationToken);
            hasEvents.ClearDomainEvents();
        }
    }
}

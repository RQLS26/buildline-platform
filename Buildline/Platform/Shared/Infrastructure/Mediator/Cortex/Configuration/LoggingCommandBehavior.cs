using Cortex.Mediator.Commands;

namespace Buildline.Platform.Shared.Infrastructure.Mediator.Cortex.Configuration;

/// <summary>
///     Cortex command pipeline behavior reserved for command logging concerns.
/// </summary>
/// <typeparam name="TCommand">Command type executed through the Cortex mediator pipeline.</typeparam>
/// <remarks>
///     The current implementation delegates immediately, but the behavior is kept as a shared
///     extension point for consistent before/after logging around command handlers.
/// </remarks>
public class LoggingCommandBehavior<TCommand>
    : ICommandPipelineBehavior<TCommand> where TCommand : ICommand
{
    /// <summary>
    ///     Executes the next command handler in the Cortex pipeline.
    /// </summary>
    /// <param name="command">Command being handled.</param>
    /// <param name="next">Delegate that invokes the next behavior or the final command handler.</param>
    /// <param name="ct">Token used to cancel command execution.</param>
    /// <returns>A task that completes when the command pipeline finishes.</returns>
    public async Task Handle(
        TCommand command,
        CommandHandlerDelegate next,
        CancellationToken ct)
    {
        await next();
    }
}

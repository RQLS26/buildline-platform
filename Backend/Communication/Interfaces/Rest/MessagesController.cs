using System.Net.Mime;
using Buildline.Platform.Communication.Application.QueryServices;
using Buildline.Platform.Communication.Domain.Model.Aggregates;
using Buildline.Platform.Communication.Domain.Repositories;
using Buildline.Platform.Communication.Interfaces.Rest.Resources;
using Buildline.Platform.Communication.Interfaces.Rest.Transform;
using Buildline.Platform.Shared.Domain.Repositories;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Buildline.Platform.Communication.Interfaces.Rest;

/// <summary>REST controller for Communication bounded context inbox endpoints.</summary>
[ApiController]
[Authorize]
[Route("api/v1/messages")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Message endpoints for alerts, operational updates and read/starred inbox state.")]
public class MessagesController(
    IMessageQueryService messageQueryService,
    IMessageRepository messageRepository,
    IUnitOfWork unitOfWork) : ControllerBase
{
    /// <summary>Gets every inbox message.</summary>
    /// <param name="cancellationToken">Token used to cancel the query.</param>
    /// <returns><c>200 OK</c> with message resources.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllMessages(CancellationToken cancellationToken)
    {
        var messages = await messageQueryService.ListAsync(cancellationToken);
        return Ok(messages.Select(MessageResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Gets one inbox message by identifier.</summary>
    /// <param name="messageId">Message identifier.</param>
    /// <param name="cancellationToken">Token used to cancel the query.</param>
    /// <returns><c>200 OK</c> when found; otherwise <c>404 Not Found</c>.</returns>
    [HttpGet("{messageId:int}")]
    public async Task<IActionResult> GetMessageById(int messageId, CancellationToken cancellationToken)
    {
        var message = await messageQueryService.FindByIdAsync(messageId, cancellationToken);
        return message is null
            ? this.NotFoundProblem("Message", messageId)
            : Ok(MessageResourceFromEntityAssembler.ToResourceFromEntity(message));
    }

    /// <summary>Creates an inbox message.</summary>
    /// <param name="resource">Message payload.</param>
    /// <param name="cancellationToken">Token used to cancel persistence.</param>
    /// <returns><c>201 Created</c> with the created message.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateMessage([FromBody] MessageWriteResource resource, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(resource.Subject))
            return this.BadRequestProblem("Message", "Subject is required.");

        var message = new Message(resource);
        await messageRepository.AddAsync(message, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return CreatedAtAction(nameof(GetMessageById), new { messageId = message.Id },
            MessageResourceFromEntityAssembler.ToResourceFromEntity(message));
    }

    /// <summary>Applies a partial inbox message update.</summary>
    /// <param name="messageId">Message identifier.</param>
    /// <param name="resource">Message fields to replace.</param>
    /// <param name="cancellationToken">Token used to cancel persistence.</param>
    /// <returns><c>200 OK</c> with the updated message.</returns>
    [HttpPatch("{messageId:int}")]
    public async Task<IActionResult> PatchMessageById(int messageId, [FromBody] MessageWriteResource resource, CancellationToken cancellationToken)
    {
        var message = await messageQueryService.FindByIdAsync(messageId, cancellationToken);
        if (message is null) return this.NotFoundProblem("Message", messageId);

        message.Apply(resource);
        messageRepository.Update(message);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Ok(MessageResourceFromEntityAssembler.ToResourceFromEntity(message));
    }

    /// <summary>Deletes an inbox message.</summary>
    /// <param name="messageId">Message identifier.</param>
    /// <param name="cancellationToken">Token used to cancel persistence.</param>
    /// <returns><c>204 No Content</c> when deletion succeeds.</returns>
    [HttpDelete("{messageId:int}")]
    public async Task<IActionResult> DeleteMessageById(int messageId, CancellationToken cancellationToken)
    {
        var message = await messageQueryService.FindByIdAsync(messageId, cancellationToken);
        if (message is null) return this.NotFoundProblem("Message", messageId);

        messageRepository.Remove(message);
        await unitOfWork.CompleteAsync(cancellationToken);
        return NoContent();
    }
}

using System.Net.Mime;
using Buildline.Platform.Communication.Application.CommandServices;
using Buildline.Platform.Communication.Application.QueryServices;
using Buildline.Platform.Communication.Interfaces.Rest.Resources;
using Buildline.Platform.Communication.Interfaces.Rest.Transform;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Buildline.Platform.Shared.Interfaces.Rest.Transform;
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
    IMessageCommandService messageCommandService,
    IMessageQueryService messageQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    /// <summary>Gets every inbox message.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAllMessages(CancellationToken cancellationToken)
    {
        var messages = await messageQueryService.ListAsync(cancellationToken);
        return Ok(messages.Select(MessageResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Gets one inbox message by identifier.</summary>
    [HttpGet("{messageId:int}")]
    public async Task<IActionResult> GetMessageById(int messageId, CancellationToken cancellationToken)
    {
        var message = await messageQueryService.FindByIdAsync(messageId, cancellationToken);
        return message is null ? this.NotFoundProblem("Message", messageId) : Ok(MessageResourceFromEntityAssembler.ToResourceFromEntity(message));
    }

    /// <summary>Creates an inbox message through the application command service.</summary>
    [HttpPost]
    public async Task<IActionResult> CreateMessage([FromBody] MessageWriteResource resource, CancellationToken cancellationToken)
    {
        var command = CreateMessageCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await messageCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            message => CreatedAtAction(nameof(GetMessageById), new { messageId = message.Id }, MessageResourceFromEntityAssembler.ToResourceFromEntity(message)));
    }

    /// <summary>Applies a partial inbox message update through the application command service.</summary>
    [HttpPatch("{messageId:int}")]
    public async Task<IActionResult> PatchMessageById(int messageId, [FromBody] MessageWriteResource resource, CancellationToken cancellationToken)
    {
        var command = UpdateMessageCommandFromResourceAssembler.ToCommandFromResource(messageId, resource);
        var result = await messageCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            message => Ok(MessageResourceFromEntityAssembler.ToResourceFromEntity(message)));
    }

    /// <summary>Deletes an inbox message through the application command service.</summary>
    [HttpDelete("{messageId:int}")]
    public async Task<IActionResult> DeleteMessageById(int messageId, CancellationToken cancellationToken)
    {
        var result = await messageCommandService.HandleDelete(messageId, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory, NoContent);
    }
}

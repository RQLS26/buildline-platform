using System.Net.Mime;
using Buildline.Platform.Communication.Application.CommandServices;
using Buildline.Platform.Communication.Application.QueryServices;
using Buildline.Platform.Communication.Interfaces.Rest.Resources;
using Buildline.Platform.Communication.Interfaces.Rest.Transform;
using Buildline.Platform.Shared.Interfaces.Rest.Company;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Buildline.Platform.Shared.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Buildline.Platform.Communication.Interfaces.Rest;

/// <summary>REST controller for company-scoped messages resources.</summary>
[ApiController]
[Authorize]
[Route("api/v1/companies/{companyId:int}/messages")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Company-scoped messages endpoints.")]
public class MessagesController(
    IMessageCommandService messageCommandService,
    IMessageQueryService messageQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    /// <summary>Gets every message owned by the route company.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAllMessages([FromRoute] int? companyId, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var items = await messageQueryService.ListByCompanyIdAsync(resolvedCompanyId, cancellationToken);
        return Ok(items.Select(MessageResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Gets one message owned by the route company.</summary>
    [HttpGet("{messageId:int}")]
    public async Task<IActionResult> GetMessageById([FromRoute] int? companyId, int messageId, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var item = await messageQueryService.FindByIdAndCompanyIdAsync(messageId, resolvedCompanyId, cancellationToken);
        return item is null
            ? this.NotFoundProblem("Message", messageId)
            : Ok(MessageResourceFromEntityAssembler.ToResourceFromEntity(item));
    }

    /// <summary>Creates a message owned by the route company.</summary>
    [HttpPost]
    public async Task<IActionResult> CreateMessage([FromRoute] int? companyId, [FromBody] MessageWriteResource resource, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var command = CreateMessageCommandFromResourceAssembler.ToCommandFromResource(resource, resolvedCompanyId);
        var result = await messageCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            item => CreatedAtAction(nameof(GetMessageById), new { companyId = resolvedCompanyId, messageId = item.Id }, MessageResourceFromEntityAssembler.ToResourceFromEntity(item)));
    }

    /// <summary>Applies a partial update to a message owned by the route company.</summary>
    [HttpPatch("{messageId:int}")]
    public async Task<IActionResult> PatchMessageById([FromRoute] int? companyId, int messageId, [FromBody] MessageWriteResource resource, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var existing = await messageQueryService.FindByIdAndCompanyIdAsync(messageId, resolvedCompanyId, cancellationToken);
        if (existing is null)
            return this.NotFoundProblem("Message", messageId);

        var command = UpdateMessageCommandFromResourceAssembler.ToCommandFromResource(messageId, resource);
        var result = await messageCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            item => Ok(MessageResourceFromEntityAssembler.ToResourceFromEntity(item)));
    }

    /// <summary>Deletes a message owned by the route company.</summary>
    [HttpDelete("{messageId:int}")]
    public async Task<IActionResult> DeleteMessageById([FromRoute] int? companyId, int messageId, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var existing = await messageQueryService.FindByIdAndCompanyIdAsync(messageId, resolvedCompanyId, cancellationToken);
        if (existing is null)
            return this.NotFoundProblem("Message", messageId);

        var result = await messageCommandService.HandleDelete(messageId, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory, NoContent);
    }
}

using System.Net.Mime;
using Buildline.Platform.Procurement.Application.CommandServices;
using Buildline.Platform.Procurement.Application.QueryServices;
using Buildline.Platform.Procurement.Interfaces.Rest.Resources;
using Buildline.Platform.Procurement.Interfaces.Rest.Transform;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Buildline.Platform.Shared.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Buildline.Platform.Procurement.Interfaces.Rest;

/// <summary>REST controller for quotation comparison workflows in the Procurement bounded context.</summary>
[ApiController]
[Authorize]
[Route("api/v1/quotations")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Quotation endpoints for supplier comparison and procurement decision workflows.")]
public class QuotationsController(
    IQuotationCommandService quotationCommandService,
    IQuotationQueryService quotationQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    /// <summary>Gets every quotation.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAllQuotations(CancellationToken cancellationToken)
    {
        var quotations = await quotationQueryService.ListAsync(cancellationToken);
        return Ok(quotations.Select(QuotationResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Gets one quotation by identifier.</summary>
    [HttpGet("{quotationId:int}")]
    public async Task<IActionResult> GetQuotationById(int quotationId, CancellationToken cancellationToken)
    {
        var quotation = await quotationQueryService.FindByIdAsync(quotationId, cancellationToken);
        return quotation is null ? this.NotFoundProblem("Quotation", quotationId) : Ok(QuotationResourceFromEntityAssembler.ToResourceFromEntity(quotation));
    }

    /// <summary>Creates a quotation through the application command service.</summary>
    [HttpPost]
    public async Task<IActionResult> CreateQuotation([FromBody] QuotationWriteResource resource, CancellationToken cancellationToken)
    {
        var command = CreateQuotationCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await quotationCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            quotation => CreatedAtAction(nameof(GetQuotationById), new { quotationId = quotation.Id }, QuotationResourceFromEntityAssembler.ToResourceFromEntity(quotation)));
    }

    /// <summary>Applies a partial quotation update through the application command service.</summary>
    [HttpPatch("{quotationId:int}")]
    public async Task<IActionResult> PatchQuotationById(int quotationId, [FromBody] QuotationWriteResource resource, CancellationToken cancellationToken)
    {
        var command = UpdateQuotationCommandFromResourceAssembler.ToCommandFromResource(quotationId, resource);
        var result = await quotationCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            quotation => Ok(QuotationResourceFromEntityAssembler.ToResourceFromEntity(quotation)));
    }
}

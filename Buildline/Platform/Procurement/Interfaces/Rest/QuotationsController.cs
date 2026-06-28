using System.Net.Mime;
using Buildline.Platform.Procurement.Application.CommandServices;
using Buildline.Platform.Procurement.Application.QueryServices;
using Buildline.Platform.Procurement.Interfaces.Rest.Resources;
using Buildline.Platform.Procurement.Interfaces.Rest.Transform;
using Buildline.Platform.Shared.Interfaces.Rest.Company;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Buildline.Platform.Shared.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Buildline.Platform.Procurement.Interfaces.Rest;

/// <summary>REST controller for company-scoped quotations resources.</summary>
[ApiController]
[Authorize]
[Route("api/v1/companies/{companyId:int}/quotations")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Company-scoped quotations endpoints.")]
public class QuotationsController(
    IQuotationCommandService quotationCommandService,
    IQuotationQueryService quotationQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    /// <summary>Gets every quotation owned by the route company.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAllQuotations([FromRoute] int? companyId, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var items = await quotationQueryService.ListByCompanyIdAsync(resolvedCompanyId, cancellationToken);
        return Ok(items.Select(QuotationResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Gets one quotation owned by the route company.</summary>
    [HttpGet("{quotationId:int}")]
    public async Task<IActionResult> GetQuotationById([FromRoute] int? companyId, int quotationId, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var item = await quotationQueryService.FindByIdAndCompanyIdAsync(quotationId, resolvedCompanyId, cancellationToken);
        return item is null
            ? this.NotFoundProblem("Quotation", quotationId)
            : Ok(QuotationResourceFromEntityAssembler.ToResourceFromEntity(item));
    }

    /// <summary>Creates a quotation owned by the route company.</summary>
    [HttpPost]
    public async Task<IActionResult> CreateQuotation([FromRoute] int? companyId, [FromBody] QuotationWriteResource resource, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var command = CreateQuotationCommandFromResourceAssembler.ToCommandFromResource(resource, resolvedCompanyId);
        var result = await quotationCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            item => CreatedAtAction(nameof(GetQuotationById), new { companyId = resolvedCompanyId, quotationId = item.Id }, QuotationResourceFromEntityAssembler.ToResourceFromEntity(item)));
    }

    /// <summary>Applies a partial update to a quotation owned by the route company.</summary>
    [HttpPatch("{quotationId:int}")]
    public async Task<IActionResult> PatchQuotationById([FromRoute] int? companyId, int quotationId, [FromBody] QuotationWriteResource resource, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var existing = await quotationQueryService.FindByIdAndCompanyIdAsync(quotationId, resolvedCompanyId, cancellationToken);
        if (existing is null)
            return this.NotFoundProblem("Quotation", quotationId);

        var command = UpdateQuotationCommandFromResourceAssembler.ToCommandFromResource(quotationId, resource);
        var result = await quotationCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            item => Ok(QuotationResourceFromEntityAssembler.ToResourceFromEntity(item)));
    }
}

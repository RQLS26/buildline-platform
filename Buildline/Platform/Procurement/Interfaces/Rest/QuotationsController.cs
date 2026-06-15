using System.Net.Mime;
using Buildline.Platform.Procurement.Application.QueryServices;
using Buildline.Platform.Procurement.Domain.Model.Aggregates;
using Buildline.Platform.Procurement.Domain.Repositories;
using Buildline.Platform.Procurement.Interfaces.Rest.Resources;
using Buildline.Platform.Procurement.Interfaces.Rest.Transform;
using Buildline.Platform.Shared.Domain.Repositories;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Buildline.Platform.Procurement.Interfaces.Rest;

/// <summary>
///     REST controller for quotation comparison workflows in the Procurement bounded context.
/// </summary>
[ApiController]
[Authorize]
[Route("api/v1/quotations")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Quotation endpoints for supplier comparison and procurement decision workflows.")]
public class QuotationsController(
    IQuotationQueryService quotationQueryService,
    IQuotationRepository quotationRepository,
    IUnitOfWork unitOfWork) : ControllerBase
{
    /// <summary>Gets every quotation.</summary>
    /// <param name="cancellationToken">Token used to cancel the query when the request is aborted.</param>
    /// <returns><c>200 OK</c> with quotation resources.</returns>
    [HttpGet]
    [SwaggerOperation(Summary = "Get all quotations", OperationId = "GetAllQuotations")]
    [SwaggerResponse(StatusCodes.Status200OK, "Quotations were returned.", typeof(IEnumerable<QuotationResource>))]
    public async Task<IActionResult> GetAllQuotations(CancellationToken cancellationToken)
    {
        var quotations = await quotationQueryService.ListAsync(cancellationToken);
        return Ok(quotations.Select(QuotationResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Gets one quotation by identifier.</summary>
    /// <param name="quotationId">Quotation persistence identifier.</param>
    /// <param name="cancellationToken">Token used to cancel the query when the request is aborted.</param>
    /// <returns><c>200 OK</c> when found; otherwise <c>404 Not Found</c>.</returns>
    [HttpGet("{quotationId:int}")]
    [SwaggerOperation(Summary = "Get quotation by id", OperationId = "GetQuotationById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Quotation was returned.", typeof(QuotationResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Quotation was not found.")]
    public async Task<IActionResult> GetQuotationById(int quotationId, CancellationToken cancellationToken)
    {
        var quotation = await quotationQueryService.FindByIdAsync(quotationId, cancellationToken);
        return quotation is null
            ? this.NotFoundProblem("Quotation", quotationId)
            : Ok(QuotationResourceFromEntityAssembler.ToResourceFromEntity(quotation));
    }

    /// <summary>Creates a quotation.</summary>
    /// <param name="resource">Quotation payload entered by logistics staff.</param>
    /// <param name="cancellationToken">Token used to cancel persistence when the request is aborted.</param>
    /// <returns><c>201 Created</c> with the created quotation resource.</returns>
    [HttpPost]
    [SwaggerOperation(Summary = "Create quotation", OperationId = "CreateQuotation")]
    [SwaggerResponse(StatusCodes.Status201Created, "Quotation was created.", typeof(QuotationResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Quotation data was invalid.")]
    public async Task<IActionResult> CreateQuotation([FromBody] QuotationWriteResource resource, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(resource.Supplier) || string.IsNullOrWhiteSpace(resource.Material))
            return this.BadRequestProblem("Quotation", "Supplier and Material are required.");

        var quotation = new Quotation(resource);
        await quotationRepository.AddAsync(quotation, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return CreatedAtAction(
            nameof(GetQuotationById),
            new { quotationId = quotation.Id },
            QuotationResourceFromEntityAssembler.ToResourceFromEntity(quotation));
    }

    /// <summary>Applies a partial quotation update.</summary>
    /// <param name="quotationId">Identifier of the quotation to update.</param>
    /// <param name="resource">Quotation fields to replace.</param>
    /// <param name="cancellationToken">Token used to cancel persistence when the request is aborted.</param>
    /// <returns><c>200 OK</c> with the updated quotation resource.</returns>
    [HttpPatch("{quotationId:int}")]
    [SwaggerOperation(Summary = "Patch quotation by id", OperationId = "PatchQuotationById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Quotation was updated.", typeof(QuotationResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Quotation was not found.")]
    public async Task<IActionResult> PatchQuotationById(
        int quotationId,
        [FromBody] QuotationWriteResource resource,
        CancellationToken cancellationToken)
    {
        var quotation = await quotationQueryService.FindByIdAsync(quotationId, cancellationToken);
        if (quotation is null) return this.NotFoundProblem("Quotation", quotationId);

        quotation.Apply(resource);
        quotationRepository.Update(quotation);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Ok(QuotationResourceFromEntityAssembler.ToResourceFromEntity(quotation));
    }
}

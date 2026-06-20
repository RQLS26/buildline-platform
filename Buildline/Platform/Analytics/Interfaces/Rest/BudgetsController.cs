using System.Net.Mime;
using Buildline.Platform.Analytics.Application.CommandServices;
using Buildline.Platform.Analytics.Application.QueryServices;
using Buildline.Platform.Analytics.Interfaces.Rest.Resources;
using Buildline.Platform.Analytics.Interfaces.Rest.Transform;
using Buildline.Platform.Shared.Interfaces.Rest.Company;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Buildline.Platform.Shared.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Buildline.Platform.Analytics.Interfaces.Rest;

/// <summary>REST controller for company-scoped budgets resources.</summary>
[ApiController]
[Authorize]
[Route("api/v1/companies/{companyId:int}/budgets")]
[Route("api/v1/budgets")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Company-scoped budgets endpoints.")]
public class BudgetsController(
    IBudgetCommandService budgetCommandService,
    IBudgetQueryService budgetQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    /// <summary>Gets every budget owned by the route company.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAllBudgets([FromRoute] int? companyId, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var items = await budgetQueryService.ListAsync(cancellationToken);
        return Ok(items.Where(item => item.CompanyId == resolvedCompanyId).Select(BudgetResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Gets one budget owned by the route company.</summary>
    [HttpGet("{budgetId:int}")]
    public async Task<IActionResult> GetBudgetById([FromRoute] int? companyId, int budgetId, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var item = await budgetQueryService.FindByIdAsync(budgetId, cancellationToken);
        return item is null || item.CompanyId != resolvedCompanyId
            ? this.NotFoundProblem("Budget", budgetId)
            : Ok(BudgetResourceFromEntityAssembler.ToResourceFromEntity(item));
    }

    /// <summary>Creates a budget owned by the route company.</summary>
    [HttpPost]
    public async Task<IActionResult> CreateBudget([FromRoute] int? companyId, [FromBody] BudgetWriteResource resource, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var command = CreateBudgetCommandFromResourceAssembler.ToCommandFromResource(resource, resolvedCompanyId);
        var result = await budgetCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            item => CreatedAtAction(nameof(GetBudgetById), new { companyId = resolvedCompanyId, budgetId = item.Id }, BudgetResourceFromEntityAssembler.ToResourceFromEntity(item)));
    }

    /// <summary>Applies a partial update to a budget owned by the route company.</summary>
    [HttpPatch("{budgetId:int}")]
    public async Task<IActionResult> PatchBudgetById([FromRoute] int? companyId, int budgetId, [FromBody] BudgetWriteResource resource, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var existing = await budgetQueryService.FindByIdAsync(budgetId, cancellationToken);
        if (existing is null || existing.CompanyId != resolvedCompanyId)
            return this.NotFoundProblem("Budget", budgetId);

        var command = UpdateBudgetCommandFromResourceAssembler.ToCommandFromResource(budgetId, resource);
        var result = await budgetCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            item => Ok(BudgetResourceFromEntityAssembler.ToResourceFromEntity(item)));
    }
}

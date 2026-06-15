using System.Net.Mime;
using Buildline.Platform.Analytics.Application.CommandServices;
using Buildline.Platform.Analytics.Application.QueryServices;
using Buildline.Platform.Analytics.Interfaces.Rest.Resources;
using Buildline.Platform.Analytics.Interfaces.Rest.Transform;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Buildline.Platform.Shared.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Buildline.Platform.Analytics.Interfaces.Rest;

/// <summary>REST controller for Analytics and Budgeting endpoints.</summary>
[ApiController]
[Authorize]
[Route("api/v1/budgets")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Budget endpoints for project cost dashboards and overrun alerts.")]
public class BudgetsController(
    IBudgetCommandService budgetCommandService,
    IBudgetQueryService budgetQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    /// <summary>Gets every project budget row.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAllBudgets(CancellationToken cancellationToken)
    {
        var budgets = await budgetQueryService.ListAsync(cancellationToken);
        return Ok(budgets.Select(BudgetResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Gets one project budget row by identifier.</summary>
    [HttpGet("{budgetId:int}")]
    public async Task<IActionResult> GetBudgetById(int budgetId, CancellationToken cancellationToken)
    {
        var budget = await budgetQueryService.FindByIdAsync(budgetId, cancellationToken);
        return budget is null ? this.NotFoundProblem("Budget", budgetId) : Ok(BudgetResourceFromEntityAssembler.ToResourceFromEntity(budget));
    }

    /// <summary>Creates a project budget row through the application command service.</summary>
    [HttpPost]
    public async Task<IActionResult> CreateBudget([FromBody] BudgetWriteResource resource, CancellationToken cancellationToken)
    {
        var command = CreateBudgetCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await budgetCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            budget => CreatedAtAction(nameof(GetBudgetById), new { budgetId = budget.Id }, BudgetResourceFromEntityAssembler.ToResourceFromEntity(budget)));
    }

    /// <summary>Applies a partial budget update through the application command service.</summary>
    [HttpPatch("{budgetId:int}")]
    public async Task<IActionResult> PatchBudgetById(int budgetId, [FromBody] BudgetWriteResource resource, CancellationToken cancellationToken)
    {
        var command = UpdateBudgetCommandFromResourceAssembler.ToCommandFromResource(budgetId, resource);
        var result = await budgetCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            budget => Ok(BudgetResourceFromEntityAssembler.ToResourceFromEntity(budget)));
    }
}

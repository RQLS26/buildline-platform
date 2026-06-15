using System.Net.Mime;
using Buildline.Platform.Analytics.Application.QueryServices;
using Buildline.Platform.Analytics.Domain.Model.Aggregates;
using Buildline.Platform.Analytics.Domain.Repositories;
using Buildline.Platform.Analytics.Interfaces.Rest.Resources;
using Buildline.Platform.Analytics.Interfaces.Rest.Transform;
using Buildline.Platform.Shared.Domain.Repositories;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
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
    IBudgetQueryService budgetQueryService,
    IBudgetRepository budgetRepository,
    IUnitOfWork unitOfWork) : ControllerBase
{
    /// <summary>Gets every project budget row.</summary>
    /// <param name="cancellationToken">Token used to cancel the query.</param>
    /// <returns><c>200 OK</c> with budget resources.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllBudgets(CancellationToken cancellationToken)
    {
        var budgets = await budgetQueryService.ListAsync(cancellationToken);
        return Ok(budgets.Select(BudgetResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Gets one project budget row by identifier.</summary>
    /// <param name="budgetId">Budget identifier.</param>
    /// <param name="cancellationToken">Token used to cancel the query.</param>
    /// <returns><c>200 OK</c> when found; otherwise <c>404 Not Found</c>.</returns>
    [HttpGet("{budgetId:int}")]
    public async Task<IActionResult> GetBudgetById(int budgetId, CancellationToken cancellationToken)
    {
        var budget = await budgetQueryService.FindByIdAsync(budgetId, cancellationToken);
        return budget is null
            ? this.NotFoundProblem("Budget", budgetId)
            : Ok(BudgetResourceFromEntityAssembler.ToResourceFromEntity(budget));
    }

    /// <summary>Creates a project budget row.</summary>
    /// <param name="resource">Budget payload.</param>
    /// <param name="cancellationToken">Token used to cancel persistence.</param>
    /// <returns><c>201 Created</c> with the created budget.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateBudget([FromBody] BudgetWriteResource resource, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(resource.Project))
            return this.BadRequestProblem("Budget", "Project is required.");

        var budget = new Budget(resource);
        await budgetRepository.AddAsync(budget, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return CreatedAtAction(nameof(GetBudgetById), new { budgetId = budget.Id },
            BudgetResourceFromEntityAssembler.ToResourceFromEntity(budget));
    }

    /// <summary>Applies a partial budget update.</summary>
    /// <param name="budgetId">Budget identifier.</param>
    /// <param name="resource">Budget fields to replace.</param>
    /// <param name="cancellationToken">Token used to cancel persistence.</param>
    /// <returns><c>200 OK</c> with the updated budget.</returns>
    [HttpPatch("{budgetId:int}")]
    public async Task<IActionResult> PatchBudgetById(int budgetId, [FromBody] BudgetWriteResource resource, CancellationToken cancellationToken)
    {
        var budget = await budgetQueryService.FindByIdAsync(budgetId, cancellationToken);
        if (budget is null) return this.NotFoundProblem("Budget", budgetId);

        budget.Apply(resource);
        budgetRepository.Update(budget);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Ok(BudgetResourceFromEntityAssembler.ToResourceFromEntity(budget));
    }
}

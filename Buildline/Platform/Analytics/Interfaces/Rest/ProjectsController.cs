using System.Net.Mime;
using Buildline.Platform.Analytics.Application.QueryServices;
using Buildline.Platform.Analytics.Interfaces.Rest.Resources;
using Buildline.Platform.Analytics.Interfaces.Rest.Transform;
using Buildline.Platform.Shared.Interfaces.Rest.Company;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Buildline.Platform.Analytics.Interfaces.Rest;

/// <summary>REST controller for company-scoped projects resources.</summary>
[ApiController]
[Authorize]
[Route("api/v1/companies/{companyId:int}/projects")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Company-scoped projects endpoints.")]
public class ProjectsController(IProjectQueryService projectQueryService) : ControllerBase
{
    /// <summary>Gets every project owned by the route company.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAllProjects([FromRoute] int? companyId, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var items = await projectQueryService.ListAsync(cancellationToken);
        return Ok(items.Where(item => item.CompanyId == resolvedCompanyId).Select(ProjectResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Gets one project owned by the route company.</summary>
    [HttpGet("{projectId:int}")]
    public async Task<IActionResult> GetProjectById([FromRoute] int? companyId, int projectId, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var item = await projectQueryService.FindByIdAsync(projectId, cancellationToken);
        return item is null || item.CompanyId != resolvedCompanyId
            ? this.NotFoundProblem("Project", projectId)
            : Ok(ProjectResourceFromEntityAssembler.ToResourceFromEntity(item));
    }
}

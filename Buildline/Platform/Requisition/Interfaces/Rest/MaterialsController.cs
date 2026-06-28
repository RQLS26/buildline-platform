using System.Net.Mime;
using Buildline.Platform.Requisition.Application.CommandServices;
using Buildline.Platform.Requisition.Application.QueryServices;
using Buildline.Platform.Requisition.Interfaces.Rest.Resources;
using Buildline.Platform.Requisition.Interfaces.Rest.Transform;
using Buildline.Platform.Shared.Interfaces.Rest.Company;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Buildline.Platform.Shared.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Buildline.Platform.Requisition.Interfaces.Rest;

/// <summary>REST controller for company-scoped material reference resources.</summary>
[ApiController]
[Authorize]
[Route("api/v1/companies/{companyId:int}/materials")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Company-scoped material reference endpoints.")]
public class MaterialsController(
    IMaterialCommandService materialCommandService,
    IMaterialQueryService materialQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    /// <summary>Gets every material owned by the route company.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAllMaterials([FromRoute] int? companyId, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var materials = await materialQueryService.ListByCompanyIdAsync(resolvedCompanyId, cancellationToken);
        return Ok(materials.Select(MaterialResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Gets one material owned by the route company.</summary>
    [HttpGet("{materialId:int}")]
    public async Task<IActionResult> GetMaterialById([FromRoute] int? companyId, int materialId, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var material = await materialQueryService.FindByIdAndCompanyIdAsync(materialId, resolvedCompanyId, cancellationToken);
        return material is null
            ? this.NotFoundProblem("Material", materialId)
            : Ok(MaterialResourceFromEntityAssembler.ToResourceFromEntity(material));
    }

    /// <summary>Creates a material owned by the route company.</summary>
    [HttpPost]
    public async Task<IActionResult> CreateMaterial([FromRoute] int? companyId, [FromBody] CreateMaterialResource resource, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var command = CreateMaterialCommandFromResourceAssembler.ToCommandFromResource(resource, resolvedCompanyId);
        var result = await materialCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            item => CreatedAtAction(nameof(GetMaterialById), new { companyId = resolvedCompanyId, materialId = item.Id }, MaterialResourceFromEntityAssembler.ToResourceFromEntity(item)));
    }

    /// <summary>Replaces material information for a record owned by the route company.</summary>
    [HttpPut("{materialId:int}")]
    public async Task<IActionResult> UpdateMaterialById([FromRoute] int? companyId, int materialId, [FromBody] UpdateMaterialResource resource, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var existing = await materialQueryService.FindByIdAndCompanyIdAsync(materialId, resolvedCompanyId, cancellationToken);
        if (existing is null)
            return this.NotFoundProblem("Material", materialId);

        var command = UpdateMaterialCommandFromResourceAssembler.ToCommandFromResource(materialId, resource);
        var result = await materialCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            item => Ok(MaterialResourceFromEntityAssembler.ToResourceFromEntity(item)));
    }

    /// <summary>Applies a PATCH-compatible material update for the route company.</summary>
    [HttpPatch("{materialId:int}")]
    public async Task<IActionResult> PatchMaterialById([FromRoute] int? companyId, int materialId, [FromBody] UpdateMaterialResource resource, CancellationToken cancellationToken)
    {
        return await UpdateMaterialById(companyId, materialId, resource, cancellationToken);
    }

    /// <summary>Deletes a material owned by the route company.</summary>
    [HttpDelete("{materialId:int}")]
    public async Task<IActionResult> DeleteMaterialById([FromRoute] int? companyId, int materialId, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var existing = await materialQueryService.FindByIdAndCompanyIdAsync(materialId, resolvedCompanyId, cancellationToken);
        if (existing is null)
            return this.NotFoundProblem("Material", materialId);

        var result = await materialCommandService.HandleDelete(materialId, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory, NoContent);
    }
}

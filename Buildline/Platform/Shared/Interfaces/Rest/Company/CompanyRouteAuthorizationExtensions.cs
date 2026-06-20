using Microsoft.AspNetCore.Mvc;

namespace Buildline.Platform.Shared.Interfaces.Rest.Company;

/// <summary>
///     Provides REST-controller helpers for company-scoped API routes.
/// </summary>
/// <remarks>
///     Operational resources in Buildline belong to a company profile. Controllers use these helpers
///     to ensure that a request to <c>/api/v1/companies/{companyId}/...</c> can only read or mutate
///     records owned by the company assigned to the authenticated JWT session.
/// </remarks>
public static class CompanyRouteAuthorizationExtensions
{
    /// <summary>
    ///     Resolves the effective company id for a request and verifies that it matches the JWT claim.
    /// </summary>
    /// <param name="controller">Controller handling the current HTTP request.</param>
    /// <param name="routeCompanyId">Company id supplied by the route, or <c>null</c> on legacy routes.</param>
    /// <param name="companyId">Resolved company id when authorization succeeds.</param>
    /// <returns><c>true</c> when the route is scoped to the authenticated company; otherwise <c>false</c>.</returns>
    public static bool TryResolveCompanyRoute(this ControllerBase controller, int? routeCompanyId, out int companyId)
    {
        companyId = 0;
        var claimValue = controller.User.FindFirst("company_id")?.Value;
        if (!int.TryParse(claimValue, out var sessionCompanyId))
            return false;

        companyId = routeCompanyId ?? sessionCompanyId;
        return companyId == sessionCompanyId;
    }
}

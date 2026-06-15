using Buildline.Platform.Suppliers.Domain.Model.Aggregates;
using Buildline.Platform.Suppliers.Interfaces.Rest.Resources;

namespace Buildline.Platform.Suppliers.Interfaces.Rest.Transform;

/// <summary>
///     Assembler that maps supplier incident aggregates to REST resources.
/// </summary>
public static class IncidentResourceFromEntityAssembler
{
    /// <summary>
    ///     Converts an incident aggregate into the frontend incident resource contract.
    /// </summary>
    /// <param name="incident">Supplier incident aggregate retrieved from persistence.</param>
    /// <returns>Supplier incident REST resource.</returns>
    public static IncidentResource ToResourceFromEntity(SupplierIncident incident)
    {
        return new IncidentResource(
            incident.Id,
            incident.IncidentId,
            incident.Title,
            incident.Description,
            incident.Supplier,
            incident.PurchaseOrder,
            incident.ReportedBy,
            incident.Severity,
            incident.Status,
            incident.Date,
            incident.Time);
    }
}

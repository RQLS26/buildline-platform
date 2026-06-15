using Buildline.Platform.Shared.Domain.Model.Events;

namespace Buildline.Platform.Suppliers.Domain.Model.Events;

/// <summary>
///     Domain event raised when SupplierIncidentReportedEvent occurs inside the aggregate boundary.
/// </summary>
public sealed record SupplierIncidentReportedEvent(int SupplierIncidentId, string IncidentId, string Supplier, string Severity) : IEvent;

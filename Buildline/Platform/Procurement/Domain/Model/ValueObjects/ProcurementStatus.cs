namespace Buildline.Platform.Procurement.Domain.Model.ValueObjects;

/// <summary>Decision states shared by quotations and purchase orders.</summary>
public enum ProcurementStatus
{
    /// <summary>The document is pending review.</summary>
    Pending,
    /// <summary>The document has been accepted or approved.</summary>
    Approved,
    /// <summary>The quotation has been accepted by procurement.</summary>
    Accepted,
    /// <summary>The document was rejected.</summary>
    Rejected
}

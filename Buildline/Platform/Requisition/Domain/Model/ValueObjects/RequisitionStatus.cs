namespace Buildline.Platform.Requisition.Domain.Model.ValueObjects;

/// <summary>Workflow states supported by the requisition board.</summary>
public enum RequisitionStatus
{
    /// <summary>The request is waiting for review.</summary>
    Pending,
    /// <summary>The request has been approved.</summary>
    Approved,
    /// <summary>The request has been rejected.</summary>
    Rejected,
    /// <summary>The requested material has been fulfilled.</summary>
    Fulfilled
}

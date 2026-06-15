namespace Buildline.Platform.Requisition.Domain.Model.ValueObjects;

/// <summary>Supported operational priorities for material requisitions.</summary>
public enum RequisitionPriority
{
    /// <summary>Low urgency requisition.</summary>
    Low,
    /// <summary>Normal urgency requisition.</summary>
    Medium,
    /// <summary>High urgency requisition.</summary>
    High
}

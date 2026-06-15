using Buildline.Platform.Procurement.Domain.Model.Commands;
using Buildline.Platform.Suppliers.Domain.Repositories;

namespace Buildline.Platform.Procurement.Application.Internal.OutboundServices;

/// <summary>
///     Supplier directory lookup used by Procurement write use cases.
/// </summary>
/// <param name="supplierRepository">Repository that exposes supplier aggregates owned by the Suppliers context.</param>
/// <remarks>
///     The frontend currently selects suppliers by company name. This service keeps that compatibility while
///     concentrating the cross-context lookup in one application-layer adapter that can later be replaced by
///     a Suppliers ACL facade without changing purchase order orchestration.
/// </remarks>
public class SupplierDirectoryService(ISupplierRepository supplierRepository) : ISupplierDirectoryService
{
    /// <inheritdoc />
    public async Task<bool> SupplierCanReceiveOrdersForAsync(
        CreatePurchaseOrderCommand command,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.SupplierName))
            return false;

        var suppliers = await supplierRepository.ListAsync(cancellationToken);
        return suppliers.Any(supplier =>
            supplier.IsActive &&
            string.Equals(supplier.CompanyName, command.SupplierName.Trim(), StringComparison.OrdinalIgnoreCase));
    }
}

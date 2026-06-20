using Buildline.Platform.Suppliers.Domain.Model.Aggregates;
using Buildline.Platform.Suppliers.Interfaces.Rest.Resources;

namespace Buildline.Platform.Suppliers.Interfaces.Rest.Transform;

/// <summary>
///     Assembler that maps supplier aggregates to REST resources.
/// </summary>
public static class SupplierResourceFromEntityAssembler
{
    /// <summary>
    ///     Converts a supplier aggregate into the frontend supplier resource contract.
    /// </summary>
    /// <param name="supplier">Supplier aggregate retrieved from persistence.</param>
    /// <returns>Supplier REST resource.</returns>
    public static SupplierResource ToResourceFromEntity(Supplier supplier)
    {
        return new SupplierResource(
            supplier.Id,
            supplier.CompanyId,
            supplier.Ruc,
            supplier.CompanyName,
            supplier.ContactName,
            supplier.Email,
            supplier.Phone,
            supplier.Rating,
            supplier.IsActive,
            supplier.Category,
            supplier.DeliveryRate);
    }
}

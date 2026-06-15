using Microsoft.EntityFrameworkCore;

namespace Buildline.Platform.Requisition.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Entity Framework Core model configuration for the Requisition bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies table mapping, constraints and seed data for material requisitions.
    /// </summary>
    /// <param name="builder">Model builder used by the shared application database context.</param>
    public static void ApplyRequisitionConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Domain.Model.Aggregates.Requisition>().HasKey(requisition => requisition.Id);
        builder.Entity<Domain.Model.Aggregates.Requisition>().Property(requisition => requisition.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Domain.Model.Aggregates.Requisition>().Property(requisition => requisition.ReqId).IsRequired().HasMaxLength(32);
        builder.Entity<Domain.Model.Aggregates.Requisition>().Property(requisition => requisition.Material).IsRequired().HasMaxLength(120);
        builder.Entity<Domain.Model.Aggregates.Requisition>().Property(requisition => requisition.Project).IsRequired().HasMaxLength(120);
        builder.Entity<Domain.Model.Aggregates.Requisition>().Property(requisition => requisition.Quantity).IsRequired();
        builder.Entity<Domain.Model.Aggregates.Requisition>().Property(requisition => requisition.Unit).IsRequired().HasMaxLength(20);
        builder.Entity<Domain.Model.Aggregates.Requisition>().Property(requisition => requisition.Priority).IsRequired().HasMaxLength(30);
        builder.Entity<Domain.Model.Aggregates.Requisition>().Property(requisition => requisition.Status).IsRequired().HasMaxLength(40);
        builder.Entity<Domain.Model.Aggregates.Requisition>().Property(requisition => requisition.RequestedOn).IsRequired().HasMaxLength(30);
        builder.Entity<Domain.Model.Aggregates.Requisition>().Property(requisition => requisition.DeliveryDate).HasMaxLength(30);
        builder.Entity<Domain.Model.Aggregates.Requisition>().Property(requisition => requisition.RequestedBy).HasMaxLength(120);

        builder.Entity<Domain.Model.Aggregates.Requisition>().HasData(
            new { Id = 1, ReqId = "MR-2026-00024", Material = "Steel Rebar 1/2\"", Project = "Skyline Tower", Quantity = 500, Unit = "PCS", Priority = "High", Status = "Pending", RequestedOn = "May 19, 2026", DeliveryDate = "2026-05-25", RequestedBy = "Carlos Mendoza" },
            new { Id = 2, ReqId = "MR-2026-00023", Material = "Concrete 3000 PSI", Project = "Skyline Tower", Quantity = 200, Unit = "Bags", Priority = "Medium", Status = "Approved", RequestedOn = "May 18, 2026", DeliveryDate = "2026-05-24", RequestedBy = "Ana Garcia" },
            new { Id = 3, ReqId = "MR-2026-00022", Material = "Sand Fine", Project = "Coastal Bridge", Quantity = 50, Unit = "m3", Priority = "Low", Status = "Approved", RequestedOn = "May 17, 2026", DeliveryDate = "2026-05-23", RequestedBy = "James Wilson" },
            new { Id = 4, ReqId = "MR-2026-00021", Material = "Cement Type I", Project = "Skyline Tower", Quantity = 300, Unit = "Bags", Priority = "High", Status = "Pending", RequestedOn = "May 17, 2026", DeliveryDate = "2026-05-22", RequestedBy = "Carlos Mendoza" });
    }
}

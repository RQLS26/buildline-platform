using Buildline.Platform.Analytics.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Buildline.Platform.Categories.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Buildline.Platform.Communication.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Buildline.Platform.Delivery.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Buildline.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Buildline.Platform.Inventory.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Buildline.Platform.Materials.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Buildline.Platform.Procurement.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Buildline.Platform.Profiles.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Buildline.Platform.Projects.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Buildline.Platform.Requisition.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;
using Buildline.Platform.Suppliers.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

/// <summary>
///     Application database context for the Buildline Platform.
/// </summary>
/// <param name="options">
///     The options for the database context
/// </param>
public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        // Apply audit timestamp interceptor for all IAuditableEntity implementations
        builder.AddInterceptors(new AuditableEntityInterceptor());
        base.OnConfiguring(builder);
    }

    /// <summary>
    ///     On creating the database model
    /// </summary>
    /// <remarks>
    ///     This method is used to create the database model for the application.
    /// </remarks>
    /// <param name="builder">
    ///     The model builder for the database context
    /// </param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Profiles Context
        builder.ApplyProfilesConfiguration();

        // Materials Context
        builder.ApplyMaterialsConfiguration();

        // Categories Context
        builder.ApplyCategoriesConfiguration();

        // Projects Context
        builder.ApplyProjectsConfiguration();

        // Requisition Context
        builder.ApplyRequisitionConfiguration();

        // Procurement Context
        builder.ApplyProcurementConfiguration();

        // Inventory Context
        builder.ApplyInventoryConfiguration();

        // Delivery & Tracking Context
        builder.ApplyDeliveryConfiguration();

        // Suppliers Context
        builder.ApplySuppliersConfiguration();

        // Analytics & Budgeting Context
        builder.ApplyAnalyticsConfiguration();

        // Communication Context
        builder.ApplyCommunicationConfiguration();

        // IAM Context
        builder.ApplyIamConfiguration();

        // General Naming Convention for the database objects
        builder.UseSnakeCaseNamingConvention();
    }
}

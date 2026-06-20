using System.Text.Json;
using Buildline.Platform.Analytics.Domain.Model.Aggregates;
using Buildline.Platform.Analytics.Domain.Model.Commands;
using Buildline.Platform.Communication.Domain.Model.Aggregates;
using Buildline.Platform.Communication.Domain.Model.Commands;
using Buildline.Platform.Delivery.Domain.Model.Commands;
using Buildline.Platform.Iam.Domain.Model.Aggregates;
using Buildline.Platform.Inventory.Domain.Model.Aggregates;
using Buildline.Platform.Inventory.Domain.Model.Commands;
using Buildline.Platform.Procurement.Domain.Model.Aggregates;
using Buildline.Platform.Procurement.Domain.Model.Commands;
using Buildline.Platform.Profiles.Domain.Model.Aggregates;
using Buildline.Platform.Requisition.Domain.Model.Aggregates;
using Buildline.Platform.Requisition.Domain.Model.Commands;
using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Buildline.Platform.Suppliers.Domain.Model.Aggregates;
using Buildline.Platform.Suppliers.Domain.Model.Commands;
using Microsoft.EntityFrameworkCore;
using DeliveryAggregate = Buildline.Platform.Delivery.Domain.Model.Aggregates.Delivery;
using RequisitionAggregate = Buildline.Platform.Requisition.Domain.Model.Aggregates.Requisition;

namespace Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Seeding;

/// <summary>
///     Seeds the local development database with frontend-compatible demonstration data.
/// </summary>
public interface IDemoDataSeeder
{
    /// <summary>
    ///     Loads demo data into empty tables when local development seeding is enabled.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel file reading and database persistence.</param>
    Task SeedAsync(CancellationToken cancellationToken = default);
}

/// <summary>
///     JSON-backed demo-data seeder for the first Web Services delivery.
/// </summary>
/// <param name="context">Application database context used to inspect and insert aggregates.</param>
/// <param name="environment">Host environment used to resolve the JSON seed file path.</param>
/// <param name="logger">Logger used to report skipped or completed seeding operations.</param>
/// <remarks>
///     EF model configuration remains limited to table mapping and constraints. Demonstration data is
///     intentionally loaded by this infrastructure service so migrations and bounded-context model
///     configuration do not contain frontend mock records.
/// </remarks>
public sealed class JsonDemoDataSeeder(
    AppDbContext context,
    IWebHostEnvironment environment,
    ILogger<JsonDemoDataSeeder> logger) : IDemoDataSeeder
{
    private const string RelativeSeedPath = "Shared/Infrastructure/Persistence/EntityFrameworkCore/Seeding/Data/demo-data.json";

    /// <inheritdoc />
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var seedPath = Path.Combine(environment.ContentRootPath, RelativeSeedPath);
        if (!File.Exists(seedPath))
        {
            logger.LogWarning("Demo seed file was not found at {SeedPath}.", seedPath);
            return;
        }

        await using var stream = File.OpenRead(seedPath);
        var data = await JsonSerializer.DeserializeAsync<DemoSeedData>(stream, JsonOptions, cancellationToken);
        if (data is null)
        {
            logger.LogWarning("Demo seed file {SeedPath} did not contain readable data.", seedPath);
            return;
        }

        var inserted = false;
        inserted |= await SeedUsersAsync(data.Users, cancellationToken);
        inserted |= await SeedProfilesAsync(data.Profiles, cancellationToken);
        inserted |= await SeedProjectsAsync(data.Projects, cancellationToken);
        inserted |= await SeedCategoriesAsync(data.Categories, cancellationToken);
        inserted |= await SeedMaterialsAsync(data.Materials, cancellationToken);
        inserted |= await SeedRequisitionsAsync(data.Requisitions, cancellationToken);
        inserted |= await SeedPurchaseOrdersAsync(data.PurchaseOrders, cancellationToken);
        inserted |= await SeedQuotationsAsync(data.Quotations, cancellationToken);
        inserted |= await SeedInventoryItemsAsync(data.InventoryItems, cancellationToken);
        inserted |= await SeedDeliveriesAsync(data.Deliveries, cancellationToken);
        inserted |= await SeedSuppliersAsync(data.Suppliers, cancellationToken);
        inserted |= await SeedSupplierIncidentsAsync(data.SupplierIncidents, cancellationToken);
        inserted |= await SeedBudgetsAsync(data.Budgets, cancellationToken);
        inserted |= await SeedMessagesAsync(data.Messages, cancellationToken);

        if (!inserted)
        {
            logger.LogInformation("Demo seed data was skipped because target tables already contain records.");
            return;
        }

        await context.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Demo seed data was loaded from {SeedPath}.", seedPath);
    }

    private static JsonSerializerOptions JsonOptions => new() { PropertyNameCaseInsensitive = true };

    private async Task<bool> SeedUsersAsync(IReadOnlyCollection<UserSeed> users, CancellationToken cancellationToken)
    {
        var existingEmails = await context.Set<User>().Select(user => user.Email).ToListAsync(cancellationToken);
        var pendingUsers = users.Where(seed => !existingEmails.Contains(seed.Email, StringComparer.OrdinalIgnoreCase)).ToList();
        if (pendingUsers.Count == 0) return false;
        context.Set<User>().AddRange(pendingUsers.Select(seed => new User(
            seed.Name,
            seed.Email,
            seed.PasswordHash,
            seed.Role,
            seed.Department,
            seed.Phone,
            seed.AvatarColor,
            seed.IsActive,
            seed.LastLogin,
            false,
            seed.CompanyId,
            seed.MembershipStatus)));
        return true;
    }

    private async Task<bool> SeedProfilesAsync(IReadOnlyCollection<ProfileSeed> profiles, CancellationToken cancellationToken)
    {
        var existingRucs = await context.Set<Profile>().Select(profile => profile.Ruc).ToListAsync(cancellationToken);
        var pendingProfiles = profiles.Where(seed => !existingRucs.Contains(seed.Ruc, StringComparer.OrdinalIgnoreCase)).ToList();
        if (pendingProfiles.Count == 0) return false;
        context.Set<Profile>().AddRange(pendingProfiles.Select(seed => new Profile(seed.CompanyName, seed.Ruc, seed.Address, seed.Phone, seed.Email)));
        return true;
    }

    private async Task<bool> SeedProjectsAsync(IReadOnlyCollection<ProjectSeed> projects, CancellationToken cancellationToken)
    {
        var existingNames = await context.Set<Project>().Select(project => project.Name).ToListAsync(cancellationToken);
        var pendingProjects = projects.Where(seed => !existingNames.Contains(seed.Name, StringComparer.OrdinalIgnoreCase)).ToList();
        if (pendingProjects.Count == 0) return false;
        context.Set<Project>().AddRange(pendingProjects.Select(seed => new Project(seed.Name, seed.Location, seed.Budget, seed.Spent, seed.Date, seed.Status, seed.Progress)));
        return true;
    }

    private async Task<bool> SeedCategoriesAsync(IReadOnlyCollection<CategorySeed> categories, CancellationToken cancellationToken)
    {
        var existingNames = await context.Set<Category>().Select(category => category.Name).ToListAsync(cancellationToken);
        var pendingCategories = categories.Where(seed => !existingNames.Contains(seed.Name, StringComparer.OrdinalIgnoreCase)).ToList();
        if (pendingCategories.Count == 0) return false;
        context.Set<Category>().AddRange(pendingCategories.Select(seed => new Category(seed.Name, seed.Description)));
        return true;
    }

    private async Task<bool> SeedMaterialsAsync(IReadOnlyCollection<MaterialSeed> materials, CancellationToken cancellationToken)
    {
        var existingSkus = await context.Set<Material>().Select(material => material.Sku).ToListAsync(cancellationToken);
        var pendingMaterials = materials.Where(seed => !existingSkus.Contains(seed.Sku, StringComparer.OrdinalIgnoreCase)).ToList();
        if (pendingMaterials.Count == 0) return false;
        context.Set<Material>().AddRange(pendingMaterials.Select(seed => new Material(new CreateMaterialCommand(
            seed.Sku,
            seed.Name,
            seed.Category,
            seed.Unit,
            seed.Project,
            seed.CurrentStock,
            seed.MinStock,
            seed.MaxStock))));
        return true;
    }

    private async Task<bool> SeedRequisitionsAsync(IReadOnlyCollection<RequisitionSeed> requisitions, CancellationToken cancellationToken)
    {
        var existingIds = await context.Set<RequisitionAggregate>().Select(requisition => requisition.ReqId).ToListAsync(cancellationToken);
        var pendingRequisitions = requisitions.Where(seed => !existingIds.Contains(seed.ReqId, StringComparer.OrdinalIgnoreCase)).ToList();
        if (pendingRequisitions.Count == 0) return false;
        context.Set<RequisitionAggregate>().AddRange(pendingRequisitions.Select(seed => new RequisitionAggregate(new CreateRequisitionCommand(
            seed.ReqId,
            seed.Material,
            seed.Project,
            seed.Quantity,
            seed.Unit,
            seed.Priority,
            seed.Status,
            seed.RequestedOn,
            seed.DeliveryDate,
            seed.RequestedBy))));
        return true;
    }

    private async Task<bool> SeedPurchaseOrdersAsync(IReadOnlyCollection<PurchaseOrderSeed> orders, CancellationToken cancellationToken)
    {
        var existingIds = await context.Set<PurchaseOrder>().Select(order => order.OrderId).ToListAsync(cancellationToken);
        var pendingOrders = orders.Where(seed => !existingIds.Contains(seed.OrderId, StringComparer.OrdinalIgnoreCase)).ToList();
        if (pendingOrders.Count == 0) return false;
        context.Set<PurchaseOrder>().AddRange(pendingOrders.Select(seed => new PurchaseOrder(new CreatePurchaseOrderCommand(
            seed.OrderId,
            seed.Date,
            seed.SupplierName,
            seed.Material,
            seed.Project,
            seed.TotalAmount,
            seed.Status))));
        return true;
    }

    private async Task<bool> SeedQuotationsAsync(IReadOnlyCollection<QuotationSeed> quotations, CancellationToken cancellationToken)
    {
        var existingIds = await context.Set<Quotation>().Select(quotation => quotation.QuotationId).ToListAsync(cancellationToken);
        var pendingQuotations = quotations.Where(seed => !existingIds.Contains(seed.QuotationId, StringComparer.OrdinalIgnoreCase)).ToList();
        if (pendingQuotations.Count == 0) return false;
        context.Set<Quotation>().AddRange(pendingQuotations.Select(seed => new Quotation(new CreateQuotationCommand(
            seed.QuotationId,
            seed.Supplier,
            seed.Material,
            seed.Project,
            seed.Amount,
            seed.Status,
            seed.Date))));
        return true;
    }

    private async Task<bool> SeedInventoryItemsAsync(IReadOnlyCollection<InventoryItemSeed> items, CancellationToken cancellationToken)
    {
        var existingSkus = await context.Set<InventoryItem>().Select(item => item.Sku).ToListAsync(cancellationToken);
        var pendingItems = items.Where(seed => !existingSkus.Contains(seed.Sku, StringComparer.OrdinalIgnoreCase)).ToList();
        if (pendingItems.Count == 0) return false;
        context.Set<InventoryItem>().AddRange(pendingItems.Select(seed => new InventoryItem(new CreateInventoryItemCommand(
            seed.Sku,
            seed.Name,
            seed.Project,
            seed.Category,
            seed.CurrentStock,
            seed.MaxStock,
            seed.MinStock,
            seed.LastUpdated))));
        return true;
    }

    private async Task<bool> SeedDeliveriesAsync(IReadOnlyCollection<DeliverySeed> deliveries, CancellationToken cancellationToken)
    {
        var existingIds = await context.Set<DeliveryAggregate>().Select(delivery => delivery.TrackingId).ToListAsync(cancellationToken);
        var pendingDeliveries = deliveries.Where(seed => !existingIds.Contains(seed.TrackingId, StringComparer.OrdinalIgnoreCase)).ToList();
        if (pendingDeliveries.Count == 0) return false;
        context.Set<DeliveryAggregate>().AddRange(pendingDeliveries.Select(seed => new DeliveryAggregate(new CreateDeliveryCommand(
            seed.TrackingId,
            seed.PurchaseOrder,
            seed.Supplier,
            seed.Origin,
            seed.Destination,
            seed.Status,
            seed.Eta,
            seed.DispatchDate,
            seed.Items,
            seed.Material))));
        return true;
    }

    private async Task<bool> SeedSuppliersAsync(IReadOnlyCollection<SupplierSeed> suppliers, CancellationToken cancellationToken)
    {
        var existingRucs = await context.Set<Supplier>().Select(supplier => supplier.Ruc).ToListAsync(cancellationToken);
        var pendingSuppliers = suppliers.Where(seed => !existingRucs.Contains(seed.Ruc, StringComparer.OrdinalIgnoreCase)).ToList();
        if (pendingSuppliers.Count == 0) return false;
        context.Set<Supplier>().AddRange(pendingSuppliers.Select(seed => new Supplier(new CreateSupplierCommand(
            seed.Ruc,
            seed.CompanyName,
            seed.ContactName,
            seed.Email,
            seed.Phone,
            seed.Rating,
            seed.IsActive,
            seed.Category,
            seed.DeliveryRate))));
        return true;
    }

    private async Task<bool> SeedSupplierIncidentsAsync(IReadOnlyCollection<SupplierIncidentSeed> incidents, CancellationToken cancellationToken)
    {
        var existingIds = await context.Set<SupplierIncident>().Select(incident => incident.IncidentId).ToListAsync(cancellationToken);
        var pendingIncidents = incidents.Where(seed => !existingIds.Contains(seed.IncidentId, StringComparer.OrdinalIgnoreCase)).ToList();
        if (pendingIncidents.Count == 0) return false;
        context.Set<SupplierIncident>().AddRange(pendingIncidents.Select(seed => new SupplierIncident(new CreateSupplierIncidentCommand(
            seed.IncidentId,
            seed.Title,
            seed.Description,
            seed.Supplier,
            seed.PurchaseOrder,
            seed.ReportedBy,
            seed.Severity,
            seed.Status,
            seed.Date,
            seed.Time))));
        return true;
    }

    private async Task<bool> SeedBudgetsAsync(IReadOnlyCollection<BudgetSeed> budgets, CancellationToken cancellationToken)
    {
        var existingProjects = await context.Set<Budget>().Select(budget => budget.Project).ToListAsync(cancellationToken);
        var pendingBudgets = budgets.Where(seed => !existingProjects.Contains(seed.Project, StringComparer.OrdinalIgnoreCase)).ToList();
        if (pendingBudgets.Count == 0) return false;
        context.Set<Budget>().AddRange(pendingBudgets.Select(seed => new Budget(new CreateBudgetCommand(
            seed.Project,
            seed.TotalBudget,
            seed.Spent,
            seed.Allocated,
            seed.Status))));
        return true;
    }

    private async Task<bool> SeedMessagesAsync(IReadOnlyCollection<MessageSeed> messages, CancellationToken cancellationToken)
    {
        var existingSubjects = await context.Set<Message>().Select(message => message.Subject).ToListAsync(cancellationToken);
        var pendingMessages = messages.Where(seed => !existingSubjects.Contains(seed.Subject, StringComparer.OrdinalIgnoreCase)).ToList();
        if (pendingMessages.Count == 0) return false;
        context.Set<Message>().AddRange(pendingMessages.Select(seed => new Message(new CreateMessageCommand(
            seed.Sender,
            seed.Subject,
            seed.Preview,
            seed.Icon,
            seed.IconClass,
            seed.Label,
            seed.LabelClass,
            seed.IsRead,
            seed.Starred,
            seed.Category,
            seed.Time,
            seed.Date))));
        return true;
    }

    private sealed record DemoSeedData(
        IReadOnlyCollection<UserSeed> Users,
        IReadOnlyCollection<ProfileSeed> Profiles,
        IReadOnlyCollection<ProjectSeed> Projects,
        IReadOnlyCollection<CategorySeed> Categories,
        IReadOnlyCollection<MaterialSeed> Materials,
        IReadOnlyCollection<RequisitionSeed> Requisitions,
        IReadOnlyCollection<PurchaseOrderSeed> PurchaseOrders,
        IReadOnlyCollection<QuotationSeed> Quotations,
        IReadOnlyCollection<InventoryItemSeed> InventoryItems,
        IReadOnlyCollection<DeliverySeed> Deliveries,
        IReadOnlyCollection<SupplierSeed> Suppliers,
        IReadOnlyCollection<SupplierIncidentSeed> SupplierIncidents,
        IReadOnlyCollection<BudgetSeed> Budgets,
        IReadOnlyCollection<MessageSeed> Messages);

    private sealed record UserSeed(string Name, string Email, string PasswordHash, string Role, string Department, string Phone, string AvatarColor, bool IsActive, string LastLogin, int? CompanyId = 1, string MembershipStatus = "active");
    private sealed record ProfileSeed(string CompanyName, string Ruc, string Address, string Phone, string Email);
    private sealed record ProjectSeed(string Name, string Location, decimal Budget, decimal Spent, string Date, string Status, int Progress);
    private sealed record CategorySeed(string Name, string Description);
    private sealed record MaterialSeed(string Sku, string Name, string Category, string Unit, string Project, int CurrentStock, int MinStock, int MaxStock);
    private sealed record RequisitionSeed(string ReqId, string Material, string Project, int Quantity, string Unit, string Priority, string Status, string RequestedOn, string DeliveryDate, string RequestedBy);
    private sealed record PurchaseOrderSeed(string OrderId, string Date, string SupplierName, string Material, string Project, decimal TotalAmount, string Status);
    private sealed record QuotationSeed(string QuotationId, string Supplier, string Material, string Project, decimal Amount, string Status, string Date);
    private sealed record InventoryItemSeed(string Sku, string Name, string Project, string Category, int CurrentStock, int MaxStock, int MinStock, string LastUpdated);
    private sealed record DeliverySeed(string TrackingId, string PurchaseOrder, string Supplier, string Origin, string Destination, string Status, string Eta, string DispatchDate, string Items, string Material);
    private sealed record SupplierSeed(string Ruc, string CompanyName, string ContactName, string Email, string Phone, int Rating, bool IsActive, string Category, int DeliveryRate);
    private sealed record SupplierIncidentSeed(string IncidentId, string Title, string Description, string Supplier, string PurchaseOrder, string ReportedBy, string Severity, string Status, string Date, string Time);
    private sealed record BudgetSeed(string Project, decimal TotalBudget, decimal Spent, decimal Allocated, string Status);
    private sealed record MessageSeed(string Sender, string Subject, string Preview, string Icon, string IconClass, string Label, string LabelClass, bool IsRead, bool Starred, string Category, string Time, string Date);
}

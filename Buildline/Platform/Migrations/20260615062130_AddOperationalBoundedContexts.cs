using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Buildline.Platform.Migrations
{
    /// <inheritdoc />
    public partial class AddOperationalBoundedContexts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "budgets",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    project = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    total_budget = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    spent = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    allocated = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    status = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_budgets", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "deliveries",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    tracking_id = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false),
                    purchase_order = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    supplier = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false),
                    origin = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false),
                    destination = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false),
                    status = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    eta = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    dispatch_date = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    items = table.Column<string>(type: "varchar(240)", maxLength: 240, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_deliveries", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "inventory_items",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    sku = table.Column<string>(type: "varchar(24)", maxLength: 24, nullable: false),
                    name = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    project = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    category = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    current_stock = table.Column<int>(type: "int", nullable: false),
                    max_stock = table.Column<int>(type: "int", nullable: false),
                    min_stock = table.Column<int>(type: "int", nullable: false),
                    last_updated = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_inventory_items", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "messages",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    sender = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    subject = table.Column<string>(type: "varchar(180)", maxLength: 180, nullable: false),
                    preview = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    icon = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    icon_class = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false),
                    label = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false),
                    label_class = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false),
                    is_read = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    starred = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    category = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    time = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    date = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_messages", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "purchase_orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    order_id = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false),
                    date = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    supplier_name = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false),
                    material = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false),
                    project = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    total_amount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    status = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_purchase_orders", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "quotations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    quotation_id = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false),
                    supplier = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false),
                    material = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false),
                    project = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    amount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    status = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    date = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_quotations", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "requisitions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    req_id = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false),
                    material = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    project = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    unit = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    priority = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    status = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    requested_on = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    delivery_date = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    requested_by = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_requisitions", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "supplier_incidents",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    incident_id = table.Column<string>(type: "varchar(24)", maxLength: 24, nullable: false),
                    title = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false),
                    description = table.Column<string>(type: "varchar(800)", maxLength: 800, nullable: false),
                    supplier = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false),
                    purchase_order = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    reported_by = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    severity = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    status = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    date = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    time = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_supplier_incidents", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "suppliers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ruc = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: false),
                    company_name = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false),
                    contact_name = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    email = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false),
                    phone = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    rating = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    category = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    delivery_rate = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_suppliers", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "budgets",
                columns: new[] { "id", "allocated", "created_at", "project", "spent", "status", "total_budget", "updated_at" },
                values: new object[,]
                {
                    { 1, 350000m, null, "Skyline Tower", 120000m, "On Track", 500000m, null },
                    { 2, 240000m, null, "Coastal Bridge", 210000m, "At Risk", 250000m, null },
                    { 3, 100000m, null, "Grand Park", 105000m, "Over Budget", 100000m, null }
                });

            migrationBuilder.InsertData(
                table: "deliveries",
                columns: new[] { "id", "created_at", "destination", "dispatch_date", "eta", "items", "origin", "purchase_order", "status", "supplier", "tracking_id", "updated_at" },
                values: new object[,]
                {
                    { 1, null, "Skyline Tower Site", "May 19, 2026", "May 21, 2026", "Steel Rebar 1/2\" (500 PCS)", "Lima Warehouse", "PO-2026-0015", "In Transit", "ABC Supplies Inc.", "TRK-0048", null },
                    { 2, null, "Coastal Bridge Site", "May 16, 2026", "May 18, 2026", "Concrete 3000 PSI (200 Bags)", "Arequipa Plant", "PO-2026-0013", "Delivered", "BuildMore Materials", "TRK-0047", null },
                    { 3, null, "Grand Park Site", "May 14, 2026", "May 17, 2026", "Steel Beams W8 (120 PCS)", "Callao Port", "PO-2026-0012", "Delayed", "Steel House Ltd.", "TRK-0046", null }
                });

            migrationBuilder.InsertData(
                table: "inventory_items",
                columns: new[] { "id", "category", "created_at", "current_stock", "last_updated", "max_stock", "min_stock", "name", "project", "sku", "updated_at" },
                values: new object[,]
                {
                    { 1, "Steel", null, 99, "2026-05-16", 800, 100, "Steel Rebar 1/2\"", "Skyline Tower", "INV-001", null },
                    { 2, "Concrete", null, 0, "2026-05-16", 500, 100, "Concrete 3000 PSI", "Skyline Tower", "INV-002", null },
                    { 3, "Concrete", null, 200, "2026-05-17", 400, 50, "Cement Type I", "Coastal Bridge", "INV-003", null },
                    { 4, "Aggregate", null, 0, "2026-05-17", 300, 50, "Sand Fine", "Grand Park", "INV-004", null }
                });

            migrationBuilder.InsertData(
                table: "messages",
                columns: new[] { "id", "category", "created_at", "date", "icon", "icon_class", "is_read", "label", "label_class", "preview", "sender", "starred", "subject", "time", "updated_at" },
                values: new object[,]
                {
                    { 1, "updates", null, "2026-05-19", "pi-check-circle", "icon-success", true, "", "", "The purchase order for ABC Supplies has been approved and is ready for dispatch.", "System", false, "PO-2026-0015 Approved", "2 min", null },
                    { 2, "alerts", null, "2026-05-19", "pi-exclamation-triangle", "icon-warning", false, "Critical", "label-critical", "Inventory at Skyline Tower dropped below minimum threshold.", "Inventory System", false, "Low Stock: Concrete 3000 PSI", "15 min", null },
                    { 3, "updates", null, "2026-05-19", "pi-truck", "icon-info", false, "", "", "Shipment from ABC Supplies has departed Lima warehouse.", "Logistics", false, "Delivery TRK-0048 In Transit", "1 hr", null }
                });

            migrationBuilder.InsertData(
                table: "purchase_orders",
                columns: new[] { "id", "created_at", "date", "material", "order_id", "project", "status", "supplier_name", "total_amount", "updated_at" },
                values: new object[,]
                {
                    { 1, null, "May 19, 2026", "Steel Rebar 1/2\"", "PO-2026-0015", "Skyline Tower", "Approved", "ABC Supplies Inc.", 125000m, null },
                    { 2, null, "May 18, 2026", "Cement Type I", "PO-2026-0014", "Skyline Tower", "Pending", "BuildMore Materials", 85200m, null },
                    { 3, null, "May 18, 2026", "Concrete 3000 PSI", "PO-2026-0013", "Coastal Bridge", "Approved", "BuildMore Materials", 42500m, null },
                    { 4, null, "May 17, 2026", "Steel Beams W8", "PO-2026-0012", "Grand Park", "Pending", "Steel House Ltd.", 67800m, null }
                });

            migrationBuilder.InsertData(
                table: "quotations",
                columns: new[] { "id", "amount", "created_at", "date", "material", "project", "quotation_id", "status", "supplier", "updated_at" },
                values: new object[,]
                {
                    { 1, 12500m, null, "May 19, 2026", "Steel Rebar 1/2\"", "Skyline Tower", "QT-2026-0018", "Pending", "ABC Supplies Inc.", null },
                    { 2, 8300m, null, "May 18, 2026", "Concrete 3000 PSI", "Skyline Tower", "QT-2026-0017", "Accepted", "Cement Plus", null },
                    { 3, 3450m, null, "May 17, 2026", "Power Drill Set", "Coastal Bridge", "QT-2026-0016", "Accepted", "Global Construction", null },
                    { 4, 15800m, null, "May 17, 2026", "Cement Type I", "Skyline Tower", "QT-2026-0015", "Pending", "BuildMore Materials", null }
                });

            migrationBuilder.InsertData(
                table: "requisitions",
                columns: new[] { "id", "created_at", "delivery_date", "material", "priority", "project", "quantity", "req_id", "requested_by", "requested_on", "status", "unit", "updated_at" },
                values: new object[,]
                {
                    { 1, null, "2026-05-25", "Steel Rebar 1/2\"", "High", "Skyline Tower", 500, "MR-2026-00024", "Carlos Mendoza", "May 19, 2026", "Pending", "PCS", null },
                    { 2, null, "2026-05-24", "Concrete 3000 PSI", "Medium", "Skyline Tower", 200, "MR-2026-00023", "Ana Garcia", "May 18, 2026", "Approved", "Bags", null },
                    { 3, null, "2026-05-23", "Sand Fine", "Low", "Coastal Bridge", 50, "MR-2026-00022", "James Wilson", "May 17, 2026", "Approved", "m3", null },
                    { 4, null, "2026-05-22", "Cement Type I", "High", "Skyline Tower", 300, "MR-2026-00021", "Carlos Mendoza", "May 17, 2026", "Pending", "Bags", null }
                });

            migrationBuilder.InsertData(
                table: "supplier_incidents",
                columns: new[] { "id", "created_at", "date", "description", "incident_id", "purchase_order", "reported_by", "severity", "status", "supplier", "time", "title", "updated_at" },
                values: new object[,]
                {
                    { 1, null, "May 15, 2026", "Steel rebar shipment from Steel House Ltd. is 3 days overdue.", "INC-015", "PO-2026-0012", "Carlos Mendoza", "High", "Open", "Steel House Ltd.", "10:45 AM", "Delayed delivery of steel rebar", null },
                    { 2, null, "May 15, 2026", "Received Type II cement instead of Type I as specified in the PO.", "INC-014", "PO-2026-0013", "Ana Garcia", "High", "In Progress", "BuildMore Materials", "08:30 AM", "Wrong cement type delivered", null },
                    { 3, null, "May 16, 2026", "Several bags arrived with torn packaging.", "INC-013", "PO-2026-0010", "James Wilson", "Medium", "Resolved", "ABC Supplies Inc.", "04:15 PM", "Damaged packaging on arrival", null }
                });

            migrationBuilder.InsertData(
                table: "suppliers",
                columns: new[] { "id", "category", "company_name", "contact_name", "created_at", "delivery_rate", "email", "is_active", "phone", "rating", "ruc", "updated_at" },
                values: new object[,]
                {
                    { 1, "Steel", "ABC Supplies Inc.", "Roberto Sanchez", null, 95, "ventas@abcsupplies.com", true, "+51 987 111 222", 5, "20100055237", null },
                    { 2, "Concrete", "BuildMore Materials", "Maria Lopez", null, 88, "contacto@buildmore.com", true, "+51 987 333 444", 4, "20419381272", null },
                    { 3, "Steel", "Steel House Ltd.", "Pedro Rojas", null, 78, "info@steelhouse.com", true, "+51 987 555 666", 4, "20555555555", null },
                    { 4, "General", "Global Construction", "Lucia Vargas", null, 82, "global@construction.com", true, "+51 987 777 888", 3, "10777777777", null },
                    { 5, "Concrete", "Cement Plus", "Jorge Diaz", null, 70, "ventas@cementplus.com", false, "+51 987 999 000", 3, "20888888888", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "budgets");

            migrationBuilder.DropTable(
                name: "deliveries");

            migrationBuilder.DropTable(
                name: "inventory_items");

            migrationBuilder.DropTable(
                name: "messages");

            migrationBuilder.DropTable(
                name: "purchase_orders");

            migrationBuilder.DropTable(
                name: "quotations");

            migrationBuilder.DropTable(
                name: "requisitions");

            migrationBuilder.DropTable(
                name: "supplier_incidents");

            migrationBuilder.DropTable(
                name: "suppliers");
        }
    }
}

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


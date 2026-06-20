using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Buildline.Platform.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(AppDbContext))]
    [Migration("20260620214500_AddCompanyScopeToOperationalRecords")]
    public partial class AddCompanyScopeToOperationalRecords : Migration
    {
        private static readonly string[] OperationalTables =
        [
            "projects",
            "budgets",
            "materials",
            "requisitions",
            "purchase_orders",
            "quotations",
            "inventory_items",
            "deliveries",
            "suppliers",
            "supplier_incidents",
            "messages"
        ];

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            foreach (var tableName in OperationalTables)
            {
                migrationBuilder.AddColumn<int>(
                    name: "company_id",
                    table: tableName,
                    type: "int",
                    nullable: false,
                    defaultValue: 1);
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            foreach (var tableName in OperationalTables)
            {
                migrationBuilder.DropColumn(
                    name: "company_id",
                    table: tableName);
            }
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Buildline.Platform.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false),
                    description = table.Column<string>(type: "varchar(180)", maxLength: 180, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_categories", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "materials",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    sku = table.Column<string>(type: "varchar(24)", maxLength: 24, nullable: false),
                    name = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    category = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false),
                    unit = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    project = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    current_stock = table.Column<int>(type: "int", nullable: false),
                    min_stock = table.Column<int>(type: "int", nullable: false),
                    max_stock = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_materials", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "profiles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    company_name = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    ruc = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: false),
                    address = table.Column<string>(type: "varchar(180)", maxLength: 180, nullable: false),
                    phone = table.Column<string>(type: "varchar(24)", maxLength: 24, nullable: false),
                    email = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_profiles", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "projects",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    location = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    budget = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    spent = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    date = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    status = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    progress = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_projects", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    email = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    password_hash = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    role = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    department = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    phone = table.Column<string>(type: "varchar(24)", maxLength: 24, nullable: false),
                    avatar_color = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: false),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    last_login = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_users", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "id", "created_at", "description", "name", "updated_at" },
                values: new object[,]
                {
                    { 1, null, "Structural steel materials such as rebar and beams.", "Steel", null },
                    { 2, null, "Cement and concrete materials for construction work.", "Concrete", null },
                    { 3, null, "Sand, gravel and related aggregate materials.", "Aggregate", null },
                    { 4, null, "Pipes and plumbing-related materials.", "Plumbing", null },
                    { 5, null, "Electrical wiring and installation materials.", "Electrical", null },
                    { 6, null, "Plywood and wood-based construction materials.", "Wood", null },
                    { 7, null, "Safety and site operation equipment.", "Equipment", null },
                    { 8, null, "General construction supplies.", "General", null }
                });

            migrationBuilder.InsertData(
                table: "materials",
                columns: new[] { "id", "category", "created_at", "current_stock", "max_stock", "min_stock", "name", "project", "sku", "unit", "updated_at" },
                values: new object[,]
                {
                    { 1, "Steel", null, 99, 800, 100, "Steel Rebar 1/2\"", "Skyline Tower", "MAT-001", "PCS", null },
                    { 2, "Concrete", null, 0, 500, 100, "Concrete 3000 PSI", "Skyline Tower", "MAT-002", "Bags", null },
                    { 3, "Concrete", null, 200, 400, 50, "Cement Type I", "Coastal Bridge", "MAT-003", "Bags", null },
                    { 4, "Aggregate", null, 0, 300, 50, "Sand Fine", "Grand Park", "MAT-004", "m3", null },
                    { 5, "Aggregate", null, 320, 500, 80, "Gravel 3/4\"", "Skyline Tower", "MAT-005", "Tons", null },
                    { 6, "Plumbing", null, 45, 200, 60, "PVC Pipes 4\"", "Coastal Bridge", "MAT-006", "PCS", null }
                });

            migrationBuilder.InsertData(
                table: "profiles",
                columns: new[] { "id", "address", "company_name", "created_at", "email", "phone", "ruc", "updated_at" },
                values: new object[] { 1, "Av. Primavera 123, Surco, Lima", "Buildline S.A.C.", null, "contacto@buildline.com", "+51 987 654 321", "20555444333", null });

            migrationBuilder.InsertData(
                table: "projects",
                columns: new[] { "id", "budget", "created_at", "date", "location", "name", "progress", "spent", "status", "updated_at" },
                values: new object[,]
                {
                    { 1, 500000m, null, "2026-01-15", "Lima, Peru", "Skyline Tower", 35, 120000m, "In Progress", null },
                    { 2, 250000m, null, "2026-03-01", "Arequipa, Peru", "Coastal Bridge", 72, 210000m, "In Progress", null },
                    { 3, 100000m, null, "2026-02-10", "Cusco, Peru", "Grand Park", 90, 105000m, "At Risk", null }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "avatar_color", "created_at", "department", "email", "is_active", "last_login", "name", "password_hash", "phone", "role", "updated_at" },
                values: new object[,]
                {
                    { 1, "#3d63a1", null, "Management", "admin@buildline.com", true, "May 19, 2026", "Nombre admin", "$2a$11$ZIgOUFd7cA0EDVQ7KXkxleNzW8rkPUHGbWp7PXuLyvOZxEWeVXQkm", "+51 987 654 321", "owner", null },
                    { 2, "#F96116", null, "Engineering", "carlos@buildline.com", true, "May 18, 2026", "Carlos Mendoza", "$2a$11$jpepzAUUxa.fRn8vJgBuQ.n4SDJFFeS/iEfg.7yjprthBADPSXbBy", "+51 912 345 678", "viewer", null }
                });

            migrationBuilder.CreateIndex(
                name: "i_x_users_email",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "materials");

            migrationBuilder.DropTable(
                name: "profiles");

            migrationBuilder.DropTable(
                name: "projects");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}

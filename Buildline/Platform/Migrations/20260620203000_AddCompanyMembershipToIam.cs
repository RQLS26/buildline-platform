using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Buildline.Platform.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(AppDbContext))]
    [Migration("20260620203000_AddCompanyMembershipToIam")]
    public partial class AddCompanyMembershipToIam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "company_id",
                table: "users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "membership_status",
                table: "users",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "active");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "company_id",
                table: "users");

            migrationBuilder.DropColumn(
                name: "membership_status",
                table: "users");
        }
    }
}

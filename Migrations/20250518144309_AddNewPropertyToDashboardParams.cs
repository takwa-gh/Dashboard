using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dashboard.Migrations
{
    /// <inheritdoc />
    public partial class AddNewPropertyToDashboardParams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ControlNumber",
                table: "DashboardParams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Family",
                table: "DashboardParams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Plant",
                table: "DashboardParams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Project",
                table: "DashboardParams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ControlNumber",
                table: "DashboardParams");

            migrationBuilder.DropColumn(
                name: "Family",
                table: "DashboardParams");

            migrationBuilder.DropColumn(
                name: "Plant",
                table: "DashboardParams");

            migrationBuilder.DropColumn(
                name: "Project",
                table: "DashboardParams");
        }
    }
}

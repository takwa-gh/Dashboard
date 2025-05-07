using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dashboard.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConveyorSpeed",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "TactTime",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "TargetQuantity",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "WorkingTime",
                table: "Stations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ConveyorSpeed",
                table: "Stations",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TactTime",
                table: "Stations",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TargetQuantity",
                table: "Stations",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "WorkingTime",
                table: "Stations",
                type: "float",
                nullable: true);
        }
    }
}

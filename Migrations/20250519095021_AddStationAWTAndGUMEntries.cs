using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dashboard.Migrations
{
    /// <inheritdoc />
    public partial class AddStationAWTAndGUMEntries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AvergeAwtValue",
                table: "Stations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "AvergeGumValue",
                table: "Stations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MaxAwtValue",
                table: "Stations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MaxGumValue",
                table: "Stations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MinAwtValue",
                table: "Stations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MinGumValue",
                table: "Stations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvergeAwtValue",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "AvergeGumValue",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "MaxAwtValue",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "MaxGumValue",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "MinAwtValue",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "MinGumValue",
                table: "Stations");
        }
    }
}

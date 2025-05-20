using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dashboard.Migrations
{
    /// <inheritdoc />
    public partial class AverageValueCorrection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AvergeGumValue",
                table: "Stations",
                newName: "AverageGumValue");

            migrationBuilder.RenameColumn(
                name: "AvergeAwtValue",
                table: "Stations",
                newName: "AverageAwtValue");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AverageGumValue",
                table: "Stations",
                newName: "AvergeGumValue");

            migrationBuilder.RenameColumn(
                name: "AverageAwtValue",
                table: "Stations",
                newName: "AvergeAwtValue");
        }
    }
}

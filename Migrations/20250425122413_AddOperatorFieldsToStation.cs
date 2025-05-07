using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dashboard.Migrations
{
    /// <inheritdoc />
    public partial class AddOperatorFieldsToStation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DirectOperator",
                table: "Stations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IndirectOperator",
                table: "Stations",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DirectOperator",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "IndirectOperator",
                table: "Stations");
        }
    }
}

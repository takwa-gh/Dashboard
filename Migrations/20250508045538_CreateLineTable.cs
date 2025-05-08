using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dashboard.Migrations
{
    /// <inheritdoc />
    public partial class CreateLineTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LineId",
                table: "Stations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Line",
                columns: table => new
                {
                    lineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TactTime = table.Column<double>(type: "float", nullable: false),
                    ConveyorSpeed = table.Column<double>(type: "float", nullable: false),
                    TargetQuantity = table.Column<double>(type: "float", nullable: false),
                    WorkingTime = table.Column<double>(type: "float", nullable: false),
                    ActualOutput = table.Column<double>(type: "float", nullable: false),
                    CycleTime = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Line", x => x.lineId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stations_LineId",
                table: "Stations",
                column: "LineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stations_Line_LineId",
                table: "Stations",
                column: "LineId",
                principalTable: "Line",
                principalColumn: "lineId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stations_Line_LineId",
                table: "Stations");

            migrationBuilder.DropTable(
                name: "Line");

            migrationBuilder.DropIndex(
                name: "IX_Stations_LineId",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "LineId",
                table: "Stations");
        }
    }
}

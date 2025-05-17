using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dashboard.Migrations
{
    /// <inheritdoc />
    public partial class linesTableToDashboardParams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stations_Lines_LineId",
                table: "Stations");

            migrationBuilder.DropTable(
                name: "Lines");

            migrationBuilder.DropIndex(
                name: "IX_Stations_LineId",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "LineId",
                table: "Stations");

            migrationBuilder.CreateTable(
                name: "DashboardParams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TactTime = table.Column<double>(type: "float", nullable: false),
                    ConveyorSpeed = table.Column<double>(type: "float", nullable: false),
                    TargetQuantity = table.Column<double>(type: "float", nullable: false),
                    WorkingTime = table.Column<double>(type: "float", nullable: false),
                    ActualOutput = table.Column<double>(type: "float", nullable: false),
                    CycleTime = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DashboardParams", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DashboardParams");

            migrationBuilder.AddColumn<int>(
                name: "LineId",
                table: "Stations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Lines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActualOutput = table.Column<double>(type: "float", nullable: false),
                    ConveyorSpeed = table.Column<double>(type: "float", nullable: false),
                    CycleTime = table.Column<double>(type: "float", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TactTime = table.Column<double>(type: "float", nullable: false),
                    TargetQuantity = table.Column<double>(type: "float", nullable: false),
                    WorkingTime = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lines", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stations_LineId",
                table: "Stations",
                column: "LineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stations_Lines_LineId",
                table: "Stations",
                column: "LineId",
                principalTable: "Lines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

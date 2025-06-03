using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dashboard.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DashboardParams");

            migrationBuilder.CreateTable(
                name: "LineParams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Plant = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Project = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Family = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ControlNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TactTime = table.Column<double>(type: "float", nullable: false),
                    ConveyorSpeed = table.Column<double>(type: "float", nullable: false),
                    TargetQuantity = table.Column<double>(type: "float", nullable: false),
                    WorkingTime = table.Column<double>(type: "float", nullable: false),
                    ActualOutput = table.Column<double>(type: "float", nullable: false),
                    CycleTime = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineParams", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LineParams");

            migrationBuilder.CreateTable(
                name: "DashboardParams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActualOutput = table.Column<double>(type: "float", nullable: false),
                    ControlNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConveyorSpeed = table.Column<double>(type: "float", nullable: false),
                    CycleTime = table.Column<double>(type: "float", nullable: false),
                    Family = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Plant = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Project = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TactTime = table.Column<double>(type: "float", nullable: false),
                    TargetQuantity = table.Column<double>(type: "float", nullable: false),
                    WorkingTime = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DashboardParams", x => x.Id);
                });
        }
    }
}

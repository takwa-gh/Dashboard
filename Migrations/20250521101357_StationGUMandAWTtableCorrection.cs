using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dashboard.Migrations
{
    /// <inheritdoc />
    public partial class StationGUMandAWTtableCorrection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_stationGUMs_Stations_StationId",
                table: "stationGUMs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_stationGUMs",
                table: "stationGUMs");

            migrationBuilder.RenameTable(
                name: "stationGUMs",
                newName: "StationGUMs");

            migrationBuilder.RenameIndex(
                name: "IX_stationGUMs_StationId",
                table: "StationGUMs",
                newName: "IX_StationGUMs_StationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StationGUMs",
                table: "StationGUMs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StationGUMs_Stations_StationId",
                table: "StationGUMs",
                column: "StationId",
                principalTable: "Stations",
                principalColumn: "StationId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StationGUMs_Stations_StationId",
                table: "StationGUMs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StationGUMs",
                table: "StationGUMs");

            migrationBuilder.RenameTable(
                name: "StationGUMs",
                newName: "stationGUMs");

            migrationBuilder.RenameIndex(
                name: "IX_StationGUMs_StationId",
                table: "stationGUMs",
                newName: "IX_stationGUMs_StationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_stationGUMs",
                table: "stationGUMs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_stationGUMs_Stations_StationId",
                table: "stationGUMs",
                column: "StationId",
                principalTable: "Stations",
                principalColumn: "StationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dashboard.Migrations
{
    /// <inheritdoc />
    public partial class bebooEnhancemant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stations_Users_UserId",
                table: "Stations");

            migrationBuilder.DropIndex(
                name: "IX_Stations_UserId",
                table: "Stations");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Stations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "Stations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Stations_UserId1",
                table: "Stations",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Stations_Users_UserId1",
                table: "Stations",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stations_Users_UserId1",
                table: "Stations");

            migrationBuilder.DropIndex(
                name: "IX_Stations_UserId1",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Stations");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Stations",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Stations_UserId",
                table: "Stations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stations_Users_UserId",
                table: "Stations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

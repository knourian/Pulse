using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pulse.Server.Migrations
{
    /// <inheritdoc />
    public partial class Update_Check_AddSigniture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedUtc",
                table: "Checks",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "Signature",
                table: "Checks",
                type: "TEXT",
                maxLength: 600,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Checks_Signature",
                table: "Checks",
                column: "Signature",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Checks_Type",
                table: "Checks",
                column: "Type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Checks_Signature",
                table: "Checks");

            migrationBuilder.DropIndex(
                name: "IX_Checks_Type",
                table: "Checks");

            migrationBuilder.DropColumn(
                name: "Signature",
                table: "Checks");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedUtc",
                table: "Checks",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pulse.Server.Migrations
{
    /// <inheritdoc />
    public partial class Add_Result : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CheckResult",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CheckId = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    AgentId = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    TimestampUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsSuccess = table.Column<bool>(type: "INTEGER", nullable: false),
                    ResponseTimeMs = table.Column<long>(type: "INTEGER", nullable: false),
                    Error = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckResult", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckResult_AgentId_TimestampUtc",
                table: "CheckResult",
                columns: new[] { "AgentId", "TimestampUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_CheckResult_CheckId_TimestampUtc",
                table: "CheckResult",
                columns: new[] { "CheckId", "TimestampUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_CheckResult_IsSuccess",
                table: "CheckResult",
                column: "IsSuccess");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckResult");
        }
    }
}

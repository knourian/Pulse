using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pulse.Server.Migrations
{
    /// <inheritdoc />
    public partial class Add_Index_CheckResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CheckResult_CheckId_AgentId_TimestampUtc",
                table: "CheckResult",
                columns: new[] { "CheckId", "AgentId", "TimestampUtc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CheckResult_CheckId_AgentId_TimestampUtc",
                table: "CheckResult");
        }
    }
}

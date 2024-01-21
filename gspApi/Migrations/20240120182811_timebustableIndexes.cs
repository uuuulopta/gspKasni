using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gspAPI.Migrations
{
    /// <inheritdoc />
    public partial class timebustableIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TimeBusTables_BusTableId",
                table: "TimeBusTables",
                column: "BusTableId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TimeBusTables_BusTableId",
                table: "TimeBusTables");
        }
    }
}

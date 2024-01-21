using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gspAPI.Migrations
{
    /// <inheritdoc />
    public partial class timebustableregistercontext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeBusTable_BusTables_BusTableId",
                table: "TimeBusTable");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeBusTable_Times_TimeId",
                table: "TimeBusTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TimeBusTable",
                table: "TimeBusTable");

            migrationBuilder.RenameTable(
                name: "TimeBusTable",
                newName: "TimeBusTables");

            migrationBuilder.RenameIndex(
                name: "IX_TimeBusTable_TimeId",
                table: "TimeBusTables",
                newName: "IX_TimeBusTables_TimeId");

            migrationBuilder.RenameIndex(
                name: "IX_TimeBusTable_BusTableId",
                table: "TimeBusTables",
                newName: "IX_TimeBusTables_BusTableId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TimeBusTables",
                table: "TimeBusTables",
                columns: new[] { "BusTableId", "TimeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TimeBusTables_BusTables_BusTableId",
                table: "TimeBusTables",
                column: "BusTableId",
                principalTable: "BusTables",
                principalColumn: "BusTableId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeBusTables_Times_TimeId",
                table: "TimeBusTables",
                column: "TimeId",
                principalTable: "Times",
                principalColumn: "TimeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeBusTables_BusTables_BusTableId",
                table: "TimeBusTables");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeBusTables_Times_TimeId",
                table: "TimeBusTables");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TimeBusTables",
                table: "TimeBusTables");

            migrationBuilder.RenameTable(
                name: "TimeBusTables",
                newName: "TimeBusTable");

            migrationBuilder.RenameIndex(
                name: "IX_TimeBusTables_TimeId",
                table: "TimeBusTable",
                newName: "IX_TimeBusTable_TimeId");

            migrationBuilder.RenameIndex(
                name: "IX_TimeBusTables_BusTableId",
                table: "TimeBusTable",
                newName: "IX_TimeBusTable_BusTableId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TimeBusTable",
                table: "TimeBusTable",
                columns: new[] { "BusTableId", "TimeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TimeBusTable_BusTables_BusTableId",
                table: "TimeBusTable",
                column: "BusTableId",
                principalTable: "BusTables",
                principalColumn: "BusTableId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeBusTable_Times_TimeId",
                table: "TimeBusTable",
                column: "TimeId",
                principalTable: "Times",
                principalColumn: "TimeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

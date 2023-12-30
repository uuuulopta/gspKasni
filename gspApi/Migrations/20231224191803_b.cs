using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gspAPI.Migrations
{
    /// <inheritdoc />
    public partial class b : Migration
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

            migrationBuilder.DropForeignKey(
                name: "FK_Times_DayType_DayTypeId",
                table: "Times");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TimeBusTable",
                table: "TimeBusTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DayType",
                table: "DayType");

            migrationBuilder.RenameTable(
                name: "TimeBusTable",
                newName: "TimeBusTables");

            migrationBuilder.RenameTable(
                name: "DayType",
                newName: "DayTypes");

            migrationBuilder.RenameIndex(
                name: "IX_TimeBusTable_TimeId",
                table: "TimeBusTables",
                newName: "IX_TimeBusTables_TimeId");

            migrationBuilder.RenameIndex(
                name: "IX_DayType_Name",
                table: "DayTypes",
                newName: "IX_DayTypes_Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TimeBusTables",
                table: "TimeBusTables",
                columns: new[] { "BusTableId", "TimeId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_DayTypes",
                table: "DayTypes",
                column: "DayTypeId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Times_DayTypes_DayTypeId",
                table: "Times",
                column: "DayTypeId",
                principalTable: "DayTypes",
                principalColumn: "DayTypeId",
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

            migrationBuilder.DropForeignKey(
                name: "FK_Times_DayTypes_DayTypeId",
                table: "Times");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TimeBusTables",
                table: "TimeBusTables");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DayTypes",
                table: "DayTypes");

            migrationBuilder.RenameTable(
                name: "TimeBusTables",
                newName: "TimeBusTable");

            migrationBuilder.RenameTable(
                name: "DayTypes",
                newName: "DayType");

            migrationBuilder.RenameIndex(
                name: "IX_TimeBusTables_TimeId",
                table: "TimeBusTable",
                newName: "IX_TimeBusTable_TimeId");

            migrationBuilder.RenameIndex(
                name: "IX_DayTypes_Name",
                table: "DayType",
                newName: "IX_DayType_Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TimeBusTable",
                table: "TimeBusTable",
                columns: new[] { "BusTableId", "TimeId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_DayType",
                table: "DayType",
                column: "DayTypeId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Times_DayType_DayTypeId",
                table: "Times",
                column: "DayTypeId",
                principalTable: "DayType",
                principalColumn: "DayTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

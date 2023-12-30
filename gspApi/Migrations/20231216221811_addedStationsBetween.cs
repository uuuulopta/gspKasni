using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gspAPI.Migrations
{
    /// <inheritdoc />
    public partial class addedStationsBetween : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PingCache_BusTables_BusTableId",
                table: "PingCache");

            migrationBuilder.DropForeignKey(
                name: "FK_PingCache_Time_TimeId",
                table: "PingCache");

            migrationBuilder.DropForeignKey(
                name: "FK_Time_DayType_DayTypeId",
                table: "Time");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeBusTable_Time_TimeId",
                table: "TimeBusTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Time",
                table: "Time");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PingCache",
                table: "PingCache");

            migrationBuilder.RenameTable(
                name: "Time",
                newName: "Times");

            migrationBuilder.RenameTable(
                name: "PingCache",
                newName: "PingCaches");

            migrationBuilder.RenameIndex(
                name: "IX_Time_DayTypeId",
                table: "Times",
                newName: "IX_Times_DayTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_PingCache_TimeId",
                table: "PingCaches",
                newName: "IX_PingCaches_TimeId");

            migrationBuilder.RenameIndex(
                name: "IX_PingCache_BusTableId",
                table: "PingCaches",
                newName: "IX_PingCaches_BusTableId");

            migrationBuilder.AlterColumn<float>(
                name: "Distance",
                table: "PingCaches",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "StationsBetween",
                table: "PingCaches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Times",
                table: "Times",
                column: "TimeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PingCaches",
                table: "PingCaches",
                column: "PingCacheId");

            migrationBuilder.AddForeignKey(
                name: "FK_PingCaches_BusTables_BusTableId",
                table: "PingCaches",
                column: "BusTableId",
                principalTable: "BusTables",
                principalColumn: "BusTableId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PingCaches_Times_TimeId",
                table: "PingCaches",
                column: "TimeId",
                principalTable: "Times",
                principalColumn: "TimeId",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PingCaches_BusTables_BusTableId",
                table: "PingCaches");

            migrationBuilder.DropForeignKey(
                name: "FK_PingCaches_Times_TimeId",
                table: "PingCaches");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeBusTable_Times_TimeId",
                table: "TimeBusTable");

            migrationBuilder.DropForeignKey(
                name: "FK_Times_DayType_DayTypeId",
                table: "Times");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Times",
                table: "Times");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PingCaches",
                table: "PingCaches");

            migrationBuilder.DropColumn(
                name: "StationsBetween",
                table: "PingCaches");

            migrationBuilder.RenameTable(
                name: "Times",
                newName: "Time");

            migrationBuilder.RenameTable(
                name: "PingCaches",
                newName: "PingCache");

            migrationBuilder.RenameIndex(
                name: "IX_Times_DayTypeId",
                table: "Time",
                newName: "IX_Time_DayTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_PingCaches_TimeId",
                table: "PingCache",
                newName: "IX_PingCache_TimeId");

            migrationBuilder.RenameIndex(
                name: "IX_PingCaches_BusTableId",
                table: "PingCache",
                newName: "IX_PingCache_BusTableId");

            migrationBuilder.AlterColumn<int>(
                name: "Distance",
                table: "PingCache",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Time",
                table: "Time",
                column: "TimeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PingCache",
                table: "PingCache",
                column: "PingCacheId");

            migrationBuilder.AddForeignKey(
                name: "FK_PingCache_BusTables_BusTableId",
                table: "PingCache",
                column: "BusTableId",
                principalTable: "BusTables",
                principalColumn: "BusTableId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PingCache_Time_TimeId",
                table: "PingCache",
                column: "TimeId",
                principalTable: "Time",
                principalColumn: "TimeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Time_DayType_DayTypeId",
                table: "Time",
                column: "DayTypeId",
                principalTable: "DayType",
                principalColumn: "DayTypeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeBusTable_Time_TimeId",
                table: "TimeBusTable",
                column: "TimeId",
                principalTable: "Time",
                principalColumn: "TimeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

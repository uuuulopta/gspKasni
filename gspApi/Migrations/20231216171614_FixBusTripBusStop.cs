using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gspAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixBusTripBusStop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusTripBusStop_BusStops_BusStopId",
                table: "BusTripBusStop");

            migrationBuilder.DropForeignKey(
                name: "FK_BusTripBusStop_BusTrips_BusTripId",
                table: "BusTripBusStop");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BusTripBusStop",
                table: "BusTripBusStop");

            migrationBuilder.RenameTable(
                name: "BusTripBusStop",
                newName: "BusTripBusStops");

            migrationBuilder.RenameIndex(
                name: "IX_BusTripBusStop_BusTripId",
                table: "BusTripBusStops",
                newName: "IX_BusTripBusStops_BusTripId");

            migrationBuilder.RenameIndex(
                name: "IX_BusTripBusStop_BusStopId",
                table: "BusTripBusStops",
                newName: "IX_BusTripBusStops_BusStopId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BusTripBusStops",
                table: "BusTripBusStops",
                column: "BusTripBusStopId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusTripBusStops_BusStops_BusStopId",
                table: "BusTripBusStops",
                column: "BusStopId",
                principalTable: "BusStops",
                principalColumn: "BusStopId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BusTripBusStops_BusTrips_BusTripId",
                table: "BusTripBusStops",
                column: "BusTripId",
                principalTable: "BusTrips",
                principalColumn: "BusTripId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusTripBusStops_BusStops_BusStopId",
                table: "BusTripBusStops");

            migrationBuilder.DropForeignKey(
                name: "FK_BusTripBusStops_BusTrips_BusTripId",
                table: "BusTripBusStops");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BusTripBusStops",
                table: "BusTripBusStops");

            migrationBuilder.RenameTable(
                name: "BusTripBusStops",
                newName: "BusTripBusStop");

            migrationBuilder.RenameIndex(
                name: "IX_BusTripBusStops_BusTripId",
                table: "BusTripBusStop",
                newName: "IX_BusTripBusStop_BusTripId");

            migrationBuilder.RenameIndex(
                name: "IX_BusTripBusStops_BusStopId",
                table: "BusTripBusStop",
                newName: "IX_BusTripBusStop_BusStopId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BusTripBusStop",
                table: "BusTripBusStop",
                column: "BusTripBusStopId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusTripBusStop_BusStops_BusStopId",
                table: "BusTripBusStop",
                column: "BusStopId",
                principalTable: "BusStops",
                principalColumn: "BusStopId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BusTripBusStop_BusTrips_BusTripId",
                table: "BusTripBusStop",
                column: "BusTripId",
                principalTable: "BusTrips",
                principalColumn: "BusTripId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

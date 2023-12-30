using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gspAPI.Migrations
{
    /// <inheritdoc />
    public partial class BusTableUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusTables_BusStop_BusStopId",
                table: "BusTables");

            migrationBuilder.DropForeignKey(
                name: "FK_BusTrip_BusRoutes_BusRouteId",
                table: "BusTrip");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BusTrip",
                table: "BusTrip");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BusStop",
                table: "BusStop");

            migrationBuilder.DropColumn(
                name: "LineNumber",
                table: "BusTables");

            migrationBuilder.RenameTable(
                name: "BusTrip",
                newName: "BusTrips");

            migrationBuilder.RenameTable(
                name: "BusStop",
                newName: "BusStops");

            migrationBuilder.RenameIndex(
                name: "IX_BusTrip_BusRouteId",
                table: "BusTrips",
                newName: "IX_BusTrips_BusRouteId");

            migrationBuilder.AddColumn<int>(
                name: "BusRouteId",
                table: "BusTables",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BusTrips",
                table: "BusTrips",
                column: "BusTripId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BusStops",
                table: "BusStops",
                column: "BusStopId");

            migrationBuilder.CreateIndex(
                name: "IX_BusTables_BusRouteId",
                table: "BusTables",
                column: "BusRouteId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusTables_BusRoutes_BusRouteId",
                table: "BusTables",
                column: "BusRouteId",
                principalTable: "BusRoutes",
                principalColumn: "BusRouteId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BusTables_BusStops_BusStopId",
                table: "BusTables",
                column: "BusStopId",
                principalTable: "BusStops",
                principalColumn: "BusStopId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BusTrips_BusRoutes_BusRouteId",
                table: "BusTrips",
                column: "BusRouteId",
                principalTable: "BusRoutes",
                principalColumn: "BusRouteId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusTables_BusRoutes_BusRouteId",
                table: "BusTables");

            migrationBuilder.DropForeignKey(
                name: "FK_BusTables_BusStops_BusStopId",
                table: "BusTables");

            migrationBuilder.DropForeignKey(
                name: "FK_BusTrips_BusRoutes_BusRouteId",
                table: "BusTrips");

            migrationBuilder.DropIndex(
                name: "IX_BusTables_BusRouteId",
                table: "BusTables");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BusTrips",
                table: "BusTrips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BusStops",
                table: "BusStops");

            migrationBuilder.DropColumn(
                name: "BusRouteId",
                table: "BusTables");

            migrationBuilder.RenameTable(
                name: "BusTrips",
                newName: "BusTrip");

            migrationBuilder.RenameTable(
                name: "BusStops",
                newName: "BusStop");

            migrationBuilder.RenameIndex(
                name: "IX_BusTrips_BusRouteId",
                table: "BusTrip",
                newName: "IX_BusTrip_BusRouteId");

            migrationBuilder.AddColumn<string>(
                name: "LineNumber",
                table: "BusTables",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BusTrip",
                table: "BusTrip",
                column: "BusTripId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BusStop",
                table: "BusStop",
                column: "BusStopId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusTables_BusStop_BusStopId",
                table: "BusTables",
                column: "BusStopId",
                principalTable: "BusStop",
                principalColumn: "BusStopId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BusTrip_BusRoutes_BusRouteId",
                table: "BusTrip",
                column: "BusRouteId",
                principalTable: "BusRoutes",
                principalColumn: "BusRouteId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

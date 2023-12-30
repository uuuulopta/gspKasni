using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gspAPI.Migrations
{
    /// <inheritdoc />
    public partial class BusTableBusStop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusTripBusStop",
                columns: table => new
                {
                    BusTripBusStopId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BusTripId = table.Column<int>(type: "int", nullable: false),
                    BusStopId = table.Column<int>(type: "int", nullable: false),
                    Direction = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusTripBusStop", x => x.BusTripBusStopId);
                    table.ForeignKey(
                        name: "FK_BusTripBusStop_BusStops_BusStopId",
                        column: x => x.BusStopId,
                        principalTable: "BusStops",
                        principalColumn: "BusStopId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusTripBusStop_BusTrips_BusTripId",
                        column: x => x.BusTripId,
                        principalTable: "BusTrips",
                        principalColumn: "BusTripId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_BusTripBusStop_BusStopId",
                table: "BusTripBusStop",
                column: "BusStopId");

            migrationBuilder.CreateIndex(
                name: "IX_BusTripBusStop_BusTripId",
                table: "BusTripBusStop",
                column: "BusTripId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusTripBusStop");
        }
    }
}

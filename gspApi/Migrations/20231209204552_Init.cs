using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace gspAPI.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BusRoutes",
                columns: table => new
                {
                    BusRouteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NameShort = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NameLong = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusRoutes", x => x.BusRouteId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BusStop",
                columns: table => new
                {
                    BusStopId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BusStopName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Lat = table.Column<double>(type: "double", nullable: false),
                    Lon = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusStop", x => x.BusStopId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DayType",
                columns: table => new
                {
                    DayTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayType", x => x.DayTypeId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BusTrip",
                columns: table => new
                {
                    BusTripId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BusRouteId = table.Column<int>(type: "int", nullable: false),
                    BusTripDirection = table.Column<int>(type: "int", nullable: false),
                    BusTripName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusTrip", x => x.BusTripId);
                    table.ForeignKey(
                        name: "FK_BusTrip_BusRoutes_BusRouteId",
                        column: x => x.BusRouteId,
                        principalTable: "BusRoutes",
                        principalColumn: "BusRouteId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BusTables",
                columns: table => new
                {
                    BusTableId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LineNumber = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Direction = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BusStopId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusTables", x => x.BusTableId);
                    table.ForeignKey(
                        name: "FK_BusTables_BusStop_BusStopId",
                        column: x => x.BusStopId,
                        principalTable: "BusStop",
                        principalColumn: "BusStopId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Time",
                columns: table => new
                {
                    TimeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Minute = table.Column<int>(type: "int", nullable: false),
                    Hour = table.Column<int>(type: "int", nullable: false),
                    DayTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Time", x => x.TimeId);
                    table.ForeignKey(
                        name: "FK_Time_DayType_DayTypeId",
                        column: x => x.DayTypeId,
                        principalTable: "DayType",
                        principalColumn: "DayTypeId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PingCache",
                columns: table => new
                {
                    PingCacheId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BusTableId = table.Column<int>(type: "int", nullable: false),
                    TimeId = table.Column<int>(type: "int", nullable: false),
                    Lat = table.Column<float>(type: "float", nullable: false),
                    Lon = table.Column<float>(type: "float", nullable: false),
                    Distance = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PingCache", x => x.PingCacheId);
                    table.ForeignKey(
                        name: "FK_PingCache_BusTables_BusTableId",
                        column: x => x.BusTableId,
                        principalTable: "BusTables",
                        principalColumn: "BusTableId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PingCache_Time_TimeId",
                        column: x => x.TimeId,
                        principalTable: "Time",
                        principalColumn: "TimeId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TimeBusTable",
                columns: table => new
                {
                    TimeId = table.Column<int>(type: "int", nullable: false),
                    BusTableId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeBusTable", x => new { x.BusTableId, x.TimeId });
                    table.ForeignKey(
                        name: "FK_TimeBusTable_BusTables_BusTableId",
                        column: x => x.BusTableId,
                        principalTable: "BusTables",
                        principalColumn: "BusTableId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimeBusTable_Time_TimeId",
                        column: x => x.TimeId,
                        principalTable: "Time",
                        principalColumn: "TimeId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "DayType",
                columns: new[] { "DayTypeId", "Name" },
                values: new object[,]
                {
                    { 1, "Workday" },
                    { 2, "Saturday" },
                    { 3, "Sunday" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusTables_BusStopId",
                table: "BusTables",
                column: "BusStopId");

            migrationBuilder.CreateIndex(
                name: "IX_BusTrip_BusRouteId",
                table: "BusTrip",
                column: "BusRouteId");

            migrationBuilder.CreateIndex(
                name: "IX_DayType_Name",
                table: "DayType",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PingCache_BusTableId",
                table: "PingCache",
                column: "BusTableId");

            migrationBuilder.CreateIndex(
                name: "IX_PingCache_TimeId",
                table: "PingCache",
                column: "TimeId");

            migrationBuilder.CreateIndex(
                name: "IX_Time_DayTypeId",
                table: "Time",
                column: "DayTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeBusTable_TimeId",
                table: "TimeBusTable",
                column: "TimeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusTrip");

            migrationBuilder.DropTable(
                name: "PingCache");

            migrationBuilder.DropTable(
                name: "TimeBusTable");

            migrationBuilder.DropTable(
                name: "BusRoutes");

            migrationBuilder.DropTable(
                name: "BusTables");

            migrationBuilder.DropTable(
                name: "Time");

            migrationBuilder.DropTable(
                name: "BusStop");

            migrationBuilder.DropTable(
                name: "DayType");
        }
    }
}

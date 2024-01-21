using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gspAPI.Migrations
{
    /// <inheritdoc />
    public partial class bustableindexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PingData",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    avg_distance = table.Column<double>(type: "double", nullable: true),
                    avg_stations_between = table.Column<double>(type: "double", nullable: true),
                    score = table.Column<double>(type: "double", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PingData", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PingData");
        }
    }
}

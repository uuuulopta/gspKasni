using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gspAPI.Migrations
{
    /// <inheritdoc />
    public partial class BusTripBusStopData : Migration
    {
        /// <inheritdoc />
          protected override void Up(MigrationBuilder migrationBuilder)
        {

            var readBuffer = File.ReadAllText("Data/stops_beg_end.sql");
            migrationBuilder.Sql(readBuffer);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql("DELETE FROM bustripbusstop");
        }
    }
}

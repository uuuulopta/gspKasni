using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gspAPI.Migrations
{
    /// <inheritdoc />
    public partial class ImportStops : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


        var readBuffer = File.ReadAllText("Data/stops.sql");
        migrationBuilder.Sql(readBuffer);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM busstop");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gspAPI.Migrations
{
    /// <inheritdoc />
    public partial class bustablebusstopdata : Migration
    {
        /// <inheritdoc />
         protected override void Up(MigrationBuilder migrationBuilder)
        {

            var readBuffer = File.ReadAllText("Data/stop_times_two.sql");
            migrationBuilder.Sql(readBuffer);
            migrationBuilder.Sql("UPDATE BusTripBusStop SET Direction=0 where Direction=1;");
            migrationBuilder.Sql("UPDATE BusTripBusStop SET Direction=1 where Direction!=0;");
            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql("DELETE FROM BusTripBusStop;");
        }
    }
}

﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gspAPI.Migrations
{
    /// <inheritdoc />
    public partial class PingCacheOppositeDirection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "GotFromOppositeDirection",
                table: "PingCaches",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GotFromOppositeDirection",
                table: "PingCaches");
        }
    }
}

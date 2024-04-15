using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Droits.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedDroitClosedDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ClosedDate",
                table: "droits",
                type: "timestamp without time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClosedDate",
                table: "droits");
        }
    }
}

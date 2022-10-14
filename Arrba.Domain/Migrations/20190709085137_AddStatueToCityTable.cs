using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Arrba.Domain.Migrations
{
    public partial class AddStatueToCityTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Cities",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Cities");
        }
    }
}

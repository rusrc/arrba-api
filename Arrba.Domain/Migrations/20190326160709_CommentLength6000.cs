using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Arrba.Domain.Migrations
{
    public partial class CommentLength6000 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "AdVehicles",
                maxLength: 6000,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2048,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "AdVehicles",
                maxLength: 2048,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 6000,
                oldNullable: true);
        }
    }
}

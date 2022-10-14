using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Arrba.Domain.Migrations
{
    public partial class AddRoleForAdminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { 1L, "52d210ca-b719-4206-b9a7-2a06f9b915fd", "Admin", null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "LastLogin", "RegistrationDate" },
                values: new object[] { new DateTime(2019, 7, 24, 13, 36, 37, 639, DateTimeKind.Local).AddTicks(1140), new DateTime(2019, 7, 24, 13, 36, 37, 638, DateTimeKind.Local).AddTicks(3410) });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { 1L, 1L });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { 1L, 1L });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "LastLogin", "RegistrationDate" },
                values: new object[] { new DateTime(2019, 7, 9, 11, 51, 37, 4, DateTimeKind.Local).AddTicks(326), new DateTime(2019, 7, 9, 11, 51, 37, 3, DateTimeKind.Local).AddTicks(2702) });
        }
    }
}

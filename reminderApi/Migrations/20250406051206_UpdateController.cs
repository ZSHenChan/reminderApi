using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace reminderApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateController : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "88cf17e7-6844-4d56-9e53-1ac3e0dbbc8b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f574934c-cd63-4897-94b4-7c3ede09ea84");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "85a0c50a-c81b-4dfd-b69b-4eeebca4984a", null, "Admin", "ADMIN" },
                    { "926cb028-050d-4fd6-bca5-9e3d2f2000ce", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "85a0c50a-c81b-4dfd-b69b-4eeebca4984a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "926cb028-050d-4fd6-bca5-9e3d2f2000ce");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "88cf17e7-6844-4d56-9e53-1ac3e0dbbc8b", null, "User", "USER" },
                    { "f574934c-cd63-4897-94b4-7c3ede09ea84", null, "Admin", "ADMIN" }
                });
        }
    }
}

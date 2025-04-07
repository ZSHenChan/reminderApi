using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace reminderApi.Migrations
{
    /// <inheritdoc />
    public partial class FixIReminder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8056f67a-999c-4c39-b052-14550756e04a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "86c57f76-2d69-4439-b081-f9b7810def86");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "474fd95e-e2e2-4c4b-b973-fa040653c31e", null, "User", "USER" },
                    { "b09dd7b9-c45f-4a8a-b1dc-612bbf0d1ac4", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "474fd95e-e2e2-4c4b-b973-fa040653c31e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b09dd7b9-c45f-4a8a-b1dc-612bbf0d1ac4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8056f67a-999c-4c39-b052-14550756e04a", null, "Admin", "ADMIN" },
                    { "86c57f76-2d69-4439-b081-f9b7810def86", null, "User", "USER" }
                });
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace reminderApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reminders_AspNetUsers_AppUserId1",
                table: "Reminders");

            migrationBuilder.DropIndex(
                name: "IX_Reminders_AppUserId1",
                table: "Reminders");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "474fd95e-e2e2-4c4b-b973-fa040653c31e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b09dd7b9-c45f-4a8a-b1dc-612bbf0d1ac4");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "Reminders");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "Reminders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5203af18-f6cd-4504-99f9-021f75d037eb", null, "Admin", "ADMIN" },
                    { "76fc8589-2009-42c0-b82a-ad986ab7bf2f", null, "User", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reminders_AppUserId",
                table: "Reminders",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reminders_AspNetUsers_AppUserId",
                table: "Reminders",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reminders_AspNetUsers_AppUserId",
                table: "Reminders");

            migrationBuilder.DropIndex(
                name: "IX_Reminders_AppUserId",
                table: "Reminders");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5203af18-f6cd-4504-99f9-021f75d037eb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "76fc8589-2009-42c0-b82a-ad986ab7bf2f");

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "Reminders",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId1",
                table: "Reminders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "474fd95e-e2e2-4c4b-b973-fa040653c31e", null, "User", "USER" },
                    { "b09dd7b9-c45f-4a8a-b1dc-612bbf0d1ac4", null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reminders_AppUserId1",
                table: "Reminders",
                column: "AppUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Reminders_AspNetUsers_AppUserId1",
                table: "Reminders",
                column: "AppUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}

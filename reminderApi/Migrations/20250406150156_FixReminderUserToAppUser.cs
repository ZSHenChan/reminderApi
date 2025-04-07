using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace reminderApi.Migrations
{
    /// <inheritdoc />
    public partial class FixReminderUserToAppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reminders_AspNetUsers_UserId1",
                table: "Reminders");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "197b2dd5-b4e1-441c-a1db-78062fc4201c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8ea26e2e-8812-45b5-9ce0-0a4885acd513");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                table: "Reminders",
                newName: "AppUserId1");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Reminders",
                newName: "AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reminders_UserId1",
                table: "Reminders",
                newName: "IX_Reminders_AppUserId1");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8056f67a-999c-4c39-b052-14550756e04a", null, "Admin", "ADMIN" },
                    { "86c57f76-2d69-4439-b081-f9b7810def86", null, "User", "USER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Reminders_AspNetUsers_AppUserId1",
                table: "Reminders",
                column: "AppUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reminders_AspNetUsers_AppUserId1",
                table: "Reminders");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8056f67a-999c-4c39-b052-14550756e04a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "86c57f76-2d69-4439-b081-f9b7810def86");

            migrationBuilder.RenameColumn(
                name: "AppUserId1",
                table: "Reminders",
                newName: "UserId1");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "Reminders",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reminders_AppUserId1",
                table: "Reminders",
                newName: "IX_Reminders_UserId1");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "197b2dd5-b4e1-441c-a1db-78062fc4201c", null, "Admin", "ADMIN" },
                    { "8ea26e2e-8812-45b5-9ce0-0a4885acd513", null, "User", "USER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Reminders_AspNetUsers_UserId1",
                table: "Reminders",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}

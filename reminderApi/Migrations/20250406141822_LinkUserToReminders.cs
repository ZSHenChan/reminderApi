using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace reminderApi.Migrations
{
    /// <inheritdoc />
    public partial class LinkUserToReminders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9012a306-e935-4739-ad60-55a131668f37");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ef01ac34-8a9c-4fba-92d8-9b42b923c915");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Reminders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Reminders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "197b2dd5-b4e1-441c-a1db-78062fc4201c", null, "Admin", "ADMIN" },
                    { "8ea26e2e-8812-45b5-9ce0-0a4885acd513", null, "User", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reminders_UserId1",
                table: "Reminders",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Reminders_AspNetUsers_UserId1",
                table: "Reminders",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reminders_AspNetUsers_UserId1",
                table: "Reminders");

            migrationBuilder.DropIndex(
                name: "IX_Reminders_UserId1",
                table: "Reminders");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "197b2dd5-b4e1-441c-a1db-78062fc4201c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8ea26e2e-8812-45b5-9ce0-0a4885acd513");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Reminders");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Reminders");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9012a306-e935-4739-ad60-55a131668f37", null, "User", "USER" },
                    { "ef01ac34-8a9c-4fba-92d8-9b42b923c915", null, "Admin", "ADMIN" }
                });
        }
    }
}

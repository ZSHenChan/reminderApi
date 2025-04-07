using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace reminderApi.Migrations
{
    /// <inheritdoc />
    public partial class RecurringPattern : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "85a0c50a-c81b-4dfd-b69b-4eeebca4984a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "926cb028-050d-4fd6-bca5-9e3d2f2000ce");

            migrationBuilder.DropColumn(
                name: "IsRecurring",
                table: "Reminders");

            migrationBuilder.DropColumn(
                name: "RepeatFrequency",
                table: "Reminders");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "DueTime",
                table: "Reminders",
                type: "time",
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "time");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DueDate",
                table: "Reminders",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<int>(
                name: "RecurringPatternId",
                table: "Reminders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RecurringPattern",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecurringType = table.Column<int>(type: "int", nullable: false),
                    Interval = table.Column<int>(type: "int", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurringPattern", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9012a306-e935-4739-ad60-55a131668f37", null, "User", "USER" },
                    { "ef01ac34-8a9c-4fba-92d8-9b42b923c915", null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reminders_RecurringPatternId",
                table: "Reminders",
                column: "RecurringPatternId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reminders_RecurringPattern_RecurringPatternId",
                table: "Reminders",
                column: "RecurringPatternId",
                principalTable: "RecurringPattern",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reminders_RecurringPattern_RecurringPatternId",
                table: "Reminders");

            migrationBuilder.DropTable(
                name: "RecurringPattern");

            migrationBuilder.DropIndex(
                name: "IX_Reminders_RecurringPatternId",
                table: "Reminders");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9012a306-e935-4739-ad60-55a131668f37");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ef01ac34-8a9c-4fba-92d8-9b42b923c915");

            migrationBuilder.DropColumn(
                name: "RecurringPatternId",
                table: "Reminders");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "DueTime",
                table: "Reminders",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0),
                oldClrType: typeof(TimeOnly),
                oldType: "time",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DueDate",
                table: "Reminders",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRecurring",
                table: "Reminders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RepeatFrequency",
                table: "Reminders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "85a0c50a-c81b-4dfd-b69b-4eeebca4984a", null, "Admin", "ADMIN" },
                    { "926cb028-050d-4fd6-bca5-9e3d2f2000ce", null, "User", "USER" }
                });
        }
    }
}

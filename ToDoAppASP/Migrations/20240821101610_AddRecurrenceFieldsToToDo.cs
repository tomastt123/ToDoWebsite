using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoAppASP.Migrations
{
    /// <inheritdoc />
    public partial class AddRecurrenceFieldsToToDo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RecurrenceEndDate",
                table: "ToDos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecurrenceFrequency",
                table: "ToDos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecurrenceInterval",
                table: "ToDos",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecurrenceEndDate",
                table: "ToDos");

            migrationBuilder.DropColumn(
                name: "RecurrenceFrequency",
                table: "ToDos");

            migrationBuilder.DropColumn(
                name: "RecurrenceInterval",
                table: "ToDos");
        }
    }
}

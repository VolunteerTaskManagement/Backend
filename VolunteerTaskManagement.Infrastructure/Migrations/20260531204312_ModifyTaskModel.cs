using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolunteerTaskManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyTaskModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                schema: "VolunteerTaskManagement",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                schema: "VolunteerTaskManagement",
                table: "Tasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                schema: "VolunteerTaskManagement",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "StartDate",
                schema: "VolunteerTaskManagement",
                table: "Tasks");
        }
    }
}

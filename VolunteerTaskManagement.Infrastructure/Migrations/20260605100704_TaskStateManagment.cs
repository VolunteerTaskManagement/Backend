using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolunteerTaskManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TaskStateManagment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "State",
                schema: "VolunteerTaskManagement",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                schema: "VolunteerTaskManagement",
                table: "Tasks");
        }
    }
}

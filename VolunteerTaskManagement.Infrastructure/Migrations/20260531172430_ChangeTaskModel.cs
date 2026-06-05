using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolunteerTaskManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTaskModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "VolunteerTaskManagement",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PicName",
                schema: "VolunteerTaskManagement",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                schema: "VolunteerTaskManagement",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "PicName",
                schema: "VolunteerTaskManagement",
                table: "Tasks");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolunteerTaskManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserProfileFeilds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                schema: "VolunteerTaskManagement",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "تاریخ تولد");

            migrationBuilder.AddColumn<long>(
                name: "NeighborhoodId",
                schema: "VolunteerTaskManagement",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                schema: "VolunteerTaskManagement",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                comment: "شماره تلفن");

            migrationBuilder.AddColumn<string>(
                name: "PicName",
                schema: "VolunteerTaskManagement",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                comment: "نام فایل تصویر پروفایل");

            migrationBuilder.AddColumn<string>(
                name: "Skills",
                schema: "VolunteerTaskManagement",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                comment: "مهارت ها");

            migrationBuilder.CreateIndex(
                name: "IX_Users_NeighborhoodId",
                schema: "VolunteerTaskManagement",
                table: "Users",
                column: "NeighborhoodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Neighborhoods_NeighborhoodId",
                schema: "VolunteerTaskManagement",
                table: "Users",
                column: "NeighborhoodId",
                principalSchema: "VolunteerTaskManagement",
                principalTable: "Neighborhoods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Neighborhoods_NeighborhoodId",
                schema: "VolunteerTaskManagement",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_NeighborhoodId",
                schema: "VolunteerTaskManagement",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                schema: "VolunteerTaskManagement",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NeighborhoodId",
                schema: "VolunteerTaskManagement",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                schema: "VolunteerTaskManagement",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PicName",
                schema: "VolunteerTaskManagement",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Skills",
                schema: "VolunteerTaskManagement",
                table: "Users");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Homework_track_API.Migrations
{
    /// <inheritdoc />
    public partial class RoleMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Role",
                schema: "Users",
                table: "Teacher",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Role",
                schema: "Users",
                table: "Student",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                schema: "Users",
                table: "Teacher");

            migrationBuilder.DropColumn(
                name: "Role",
                schema: "Users",
                table: "Student");
        }
    }
}

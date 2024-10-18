using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Homework_track_API.Migrations
{
    /// <inheritdoc />
    public partial class EditedMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Mark",
                schema: "Operations",
                table: "Submission",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "Courses",
                table: "Course",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mark",
                schema: "Operations",
                table: "Submission");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "Courses",
                table: "Course");
        }
    }
}

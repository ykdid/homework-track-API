using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Homework_track_API.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentNumber",
                schema: "Users",
                table: "Student");

            migrationBuilder.EnsureSchema(
                name: "Courses");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                schema: "Operations",
                table: "Homework",
                newName: "CourseId");

            migrationBuilder.RenameColumn(
                name: "HomeworkImagePath",
                schema: "Operations",
                table: "Homework",
                newName: "ImagePath");

            migrationBuilder.RenameColumn(
                name: "HomeworkDocumentationPath",
                schema: "Operations",
                table: "Homework",
                newName: "DocumentationPath");

            migrationBuilder.CreateTable(
                name: "Course",
                schema: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TeacherId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Course_Teacher_TeacherId",
                        column: x => x.TeacherId,
                        principalSchema: "Users",
                        principalTable: "Teacher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentCourse",
                schema: "Courses",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    CourseId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentCourse", x => new { x.StudentId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_StudentCourse_Course_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "Courses",
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentCourse_Student_StudentId",
                        column: x => x.StudentId,
                        principalSchema: "Users",
                        principalTable: "Student",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Homework_CourseId",
                schema: "Operations",
                table: "Homework",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Course_TeacherId",
                schema: "Courses",
                table: "Course",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourse_CourseId",
                schema: "Courses",
                table: "StudentCourse",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Homework_Course_CourseId",
                schema: "Operations",
                table: "Homework",
                column: "CourseId",
                principalSchema: "Courses",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homework_Course_CourseId",
                schema: "Operations",
                table: "Homework");

            migrationBuilder.DropTable(
                name: "StudentCourse",
                schema: "Courses");

            migrationBuilder.DropTable(
                name: "Course",
                schema: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Homework_CourseId",
                schema: "Operations",
                table: "Homework");

            migrationBuilder.RenameColumn(
                name: "ImagePath",
                schema: "Operations",
                table: "Homework",
                newName: "HomeworkImagePath");

            migrationBuilder.RenameColumn(
                name: "DocumentationPath",
                schema: "Operations",
                table: "Homework",
                newName: "HomeworkDocumentationPath");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                schema: "Operations",
                table: "Homework",
                newName: "TeacherId");

            migrationBuilder.AddColumn<string>(
                name: "StudentNumber",
                schema: "Users",
                table: "Student",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}

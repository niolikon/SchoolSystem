using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSystem.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class FixedCourseTeacherAgainAgainAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoursesEnrolledStudents_Courses_CoursesId",
                table: "CoursesEnrolledStudents");

            migrationBuilder.RenameColumn(
                name: "CoursesId",
                table: "CoursesEnrolledStudents",
                newName: "CourseModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoursesEnrolledStudents_Courses_CourseModelId",
                table: "CoursesEnrolledStudents",
                column: "CourseModelId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoursesEnrolledStudents_Courses_CourseModelId",
                table: "CoursesEnrolledStudents");

            migrationBuilder.RenameColumn(
                name: "CourseModelId",
                table: "CoursesEnrolledStudents",
                newName: "CoursesId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoursesEnrolledStudents_Courses_CoursesId",
                table: "CoursesEnrolledStudents",
                column: "CoursesId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

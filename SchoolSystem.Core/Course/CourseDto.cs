using SchoolSystem.Core.Student;
using SchoolSystem.Core.Teacher;
using System.ComponentModel.DataAnnotations;


namespace SchoolSystem.Core.Course;

public class CourseDto
{
    public int Id { get; set; }

    [Required, StringLength(maximumLength: 60, MinimumLength = 3)]
    public required string Name { get; set; }

    [Required, Range(1, 20)]
    public required int Credits { get; set; }

    [Required]
    public required int TeacherId { get; set; }
    public TeacherDto? Teacher { get; set; }

    public List<StudentDto>? EnrolledStudents { get; set; }
}

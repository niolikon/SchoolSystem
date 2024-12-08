using SchoolSystem.Core.Course;
using SchoolSystem.Core.Student;
using System.ComponentModel.DataAnnotations;


namespace SchoolSystem.Core.CourseEnrollment;

public class CourseEnrollmentDto
{
    public int Id { get; set; }

    [Required]
    public int StudentId { get; set; }
    public StudentDto? Student { get; set; }

    [Required]
    public int CourseId { get; set; }
    public CourseDto? Course { get; set; }

    public DateTime EnrollmentDate { get; set; }
}

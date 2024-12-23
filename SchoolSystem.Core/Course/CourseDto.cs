using SchoolSystem.Core.Student;
using SchoolSystem.Core.Teacher;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace SchoolSystem.Core.Course;

public class CourseDto
{
    public int Id { get; set; }

    [Required, StringLength(maximumLength: 60, MinimumLength = 3)]
    public required string Name { get; set; }

    [Required, Range(1, 20)]
    public required int Credits { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? TeacherId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TeacherDto? Teacher { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<StudentDto>? Students { get; set; }
}

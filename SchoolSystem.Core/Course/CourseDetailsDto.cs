using SchoolSystem.Core.Student;
using SchoolSystem.Core.Teacher;
using System.Text.Json.Serialization;

namespace SchoolSystem.Core.Course;

public class CourseDetailsDto
{
    public required int Id { get; set; }

    public required string Name { get; set; }

    public required int Credits { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? TeacherId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TeacherDetailsDto? Teacher { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<StudentDetailsDto>? Students { get; set; }
}

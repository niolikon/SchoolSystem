using SchoolSystem.Core.Course;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SchoolSystem.Core.Teacher;

public class TeacherDetailsDto
{
    public int Id { get; set; }

    [Required, StringLength(maximumLength: 100, MinimumLength = 2)]
    public required string FullName { get; set; }

    [Required, StringLength(maximumLength: 20, MinimumLength = 2)]
    public required string Position { get; set; }

    [Required, DataType(DataType.EmailAddress), StringLength(maximumLength: 100, MinimumLength = 5)]
    public required string Email { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<CourseDetailsDto>? Courses { get; set; }
}

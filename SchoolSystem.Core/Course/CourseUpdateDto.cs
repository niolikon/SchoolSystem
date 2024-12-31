using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SchoolSystem.Core.Course;

public class CourseUpdateDto
{
    [Required]
    public required int Id { get; set; }

    [Required, StringLength(maximumLength: 60, MinimumLength = 3)]
    public required string Name { get; set; }

    [Required, Range(1, 20)]
    public required int Credits { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? TeacherId { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Core.Teacher;

public class TeacherCreateDto
{
    [Required, StringLength(maximumLength: 100, MinimumLength = 2)]
    public required string FullName { get; set; }

    [Required, StringLength(maximumLength: 20, MinimumLength = 2)]
    public required string Position { get; set; }

    [Required, DataType(DataType.EmailAddress), StringLength(maximumLength: 100, MinimumLength = 5)]
    public required string Email { get; set; }
}

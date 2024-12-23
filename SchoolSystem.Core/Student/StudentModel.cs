using SchoolSystem.Core.Common.BaseClasses;
using SchoolSystem.Core.Course;
using System.ComponentModel.DataAnnotations;


namespace SchoolSystem.Core.Student;

public class StudentModel : BaseModel<int>
{
    [Required, StringLength(maximumLength: 100, MinimumLength = 2)]
    public required string FullName { get; set; }

    [Required, DataType(DataType.EmailAddress), StringLength(maximumLength: 100, MinimumLength = 5)]
    public required string Email { get; set; }

    public virtual List<CourseModel>? Courses { get; set; }
}

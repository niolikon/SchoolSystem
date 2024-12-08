using SchoolSystem.Core.Common.BaseClasses;
using SchoolSystem.Core.CourseEnrollment;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SchoolSystem.Core.Teacher;

[Table("Teachers")]
public class TeacherModel : BaseModel<int>
{
    [Required, StringLength(maximumLength: 100, MinimumLength = 2)]
    public required string FullName { get; set; }

    [Required, StringLength(maximumLength: 20, MinimumLength = 2)]
    public required string Position { get; set; }

    [Required, DataType(DataType.EmailAddress), StringLength(maximumLength: 100, MinimumLength = 5)]
    public required string Email { get; set; }

    public virtual ICollection<CourseEnrollmentModel>? Courses { get; set; }
}

using SchoolSystem.Core.Common.BaseClasses;
using SchoolSystem.Core.CourseEnrollment;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SchoolSystem.Core.Student;

[Table("Students")]
public class StudentModel : BaseModel<int>
{
    [Required, StringLength(maximumLength: 100, MinimumLength = 2)]
    public required string FullName { get; set; }

    [Required, DataType(DataType.EmailAddress), StringLength(maximumLength: 100, MinimumLength = 5)]
    public required string Email { get; set; }

    public virtual ICollection<CourseEnrollmentModel>? Enrollments { get; set; }
}

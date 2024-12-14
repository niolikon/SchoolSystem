using SchoolSystem.Core.Common.BaseClasses;
using SchoolSystem.Core.Teacher;
using SchoolSystem.Core.CourseEnrollment;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SchoolSystem.Core.Course;

[Table("Courses")]
public class CourseModel : BaseModel<int>
{
    [Required, StringLength(maximumLength: 60, MinimumLength = 3)]
    public required string Name { get; set; }

    [Required, Range(1, 20)]
    public required int Credits { get; set; }

    [Required]
    public int TeacherId { get; set; }
    [ForeignKey(nameof(TeacherId))]
    public virtual TeacherModel? Teacher { get; set; }

    public virtual ICollection<CourseEnrollmentModel>? Enrollments { get; set; }

    public CourseModel Clone => (CourseModel) MemberwiseClone();
}

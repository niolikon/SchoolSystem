using SchoolSystem.Core.Common.BaseClasses;
using SchoolSystem.Core.Teacher;
using SchoolSystem.Core.Student;
using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Core.Course;

public class CourseModel : BaseModel<int>
{
    [Required, StringLength(maximumLength: 60, MinimumLength = 3)]
    public required string Name { get; set; }

    [Required, Range(1, 20)]
    public required int Credits { get; set; }

    public int? TeacherId { get; set; }

    public virtual TeacherModel? Teacher { get; set; }

    public virtual List<StudentModel> Students { get; set; } = [];
}

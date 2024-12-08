using SchoolSystem.Core.Common.BaseClasses;
using SchoolSystem.Core.Student;
using SchoolSystem.Core.Course;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSystem.Core.CourseEnrollment
{
    [Table("CourseEnrollments")]
    public class CourseEnrollmentModel : BaseModel<int>
    {
        [Required]
        public required int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public virtual StudentModel? Student { get; set; }

        [Required]
        public required int CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public virtual CourseModel? Course { get; set; }

        public DateTime EnrollmentDate { get; set; }
    }
}

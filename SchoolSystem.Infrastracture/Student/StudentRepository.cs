using SchoolSystem.Infrastracture.Common.BaseClasses;
using SchoolSystem.Infrastracture.Common;
using SchoolSystem.Core.Student;
using SchoolSystem.Core.CourseEnrollment;
using Microsoft.EntityFrameworkCore;

namespace SchoolSystem.Infrastracture.Student;

public class StudentRepository : BaseRepository<StudentModel>, IStudentRepository
{
    public StudentRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<StudentModel>> FindStudentsByCourseId(int courseId)
    {
        return await _dbContext.Set<CourseEnrollmentModel>()
            // Filtering enrollments by courseId
            .Where(enrollment => enrollment.CourseId == courseId)
            // Join ..
            .Join(
                // ... with table related to StudentModel ...
                _dbContext.Set<StudentModel>(),
                // ... using CourseId as join key for CourseEnrollment ...
                enrollment => enrollment.StudentId,
                // ... and Id as join key for StudentModel ...
                student => student.Id,
                // Results projection, for each couple enrollment, student => Take the student
                (enrollment, student) => student
            )
            .ToListAsync();
    }
}

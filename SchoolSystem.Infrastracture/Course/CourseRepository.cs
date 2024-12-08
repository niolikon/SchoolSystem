using SchoolSystem.Infrastracture.Common.BaseClasses;
using SchoolSystem.Infrastracture.Common;
using SchoolSystem.Core.Course;
using SchoolSystem.Core.CourseEnrollment;
using Microsoft.EntityFrameworkCore;

namespace SchoolSystem.Infrastracture.Course;

public class CourseRepository : BaseRepository<CourseModel>, ICourseRepository
{
    public CourseRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<CourseModel>> FindCoursesByStudentId(int studentId)
    {
        return await _dbContext.Set<CourseEnrollmentModel>()
            // Filtering enrollments by studentId
            .Where(enrollment => enrollment.StudentId == studentId) 
            // Join ..
            .Join(
                // ... with table related to CourseModel ...
                _dbContext.Set<CourseModel>(), 
                // ... using CourseId as join key for CourseEnrollment ...
                enrollment => enrollment.CourseId,
                // ... and Id as join key for CourseModel ...
                course => course.Id,
                // Results projection, for each couple enrollment, course => Take the course
                (enrollment, course) => course 
            )
            .ToListAsync();
    }

    public async Task<IEnumerable<CourseModel>> FindCoursesByTeacherId(int teacherId)
    {
        return await _dbContext.Set<CourseModel>()
            .Where(course => course.TeacherId == teacherId)
            .Select(course => course)
            .ToListAsync();
    }
}

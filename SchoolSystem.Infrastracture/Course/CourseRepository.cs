using SchoolSystem.Infrastracture.Common.BaseClasses;
using SchoolSystem.Infrastracture.Common;
using SchoolSystem.Core.Course;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Core.Student;

namespace SchoolSystem.Infrastracture.Course;

public class CourseRepository : BaseRepository<CourseModel, int>, ICourseRepository
{
    public CourseRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<CourseModel>> FindCoursesByStudentId(int studentId)
    {
        IQueryable<CourseModel> courses = _dbContext.Set<CourseModel>();

        IQueryable<CourseModel> query = 
            from c in courses
            from s in c.Students
            where s.Id == studentId
            select c;
        
        return await query.ToListAsync();
    }

    public async Task<IEnumerable<CourseModel>> FindCoursesByTeacherId(int teacherId)
    {
        IQueryable<CourseModel> courses = _dbContext.Set<CourseModel>();

        IQueryable<CourseModel> query =
            from c in courses
            where c.TeacherId == teacherId
            select c;
        
        return await query.ToListAsync();
    }
}

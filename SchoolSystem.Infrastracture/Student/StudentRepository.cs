using SchoolSystem.Infrastracture.Common.BaseClasses;
using SchoolSystem.Infrastracture.Common;
using SchoolSystem.Core.Student;
using Microsoft.EntityFrameworkCore;

namespace SchoolSystem.Infrastracture.Student;

public class StudentRepository : BaseRepository<StudentModel, int>, IStudentRepository
{
    public StudentRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<StudentModel>> FindStudentsByCourseId(int courseId)
    {
        IQueryable<StudentModel> students = _dbContext.Set<StudentModel>();

        IQueryable<StudentModel> query = 
            from s in students
            from c in s.Courses
            where c.Id == courseId
            select s;
        
        return await query.ToListAsync();
    }
}

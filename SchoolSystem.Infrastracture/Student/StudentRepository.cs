using SchoolSystem.Infrastracture.Common.BaseClasses;
using SchoolSystem.Infrastracture.Common;
using SchoolSystem.Core.Student;

namespace SchoolSystem.Infrastracture.Student;

public class StudentRepository : BaseRepository<StudentModel>, IStudentRepository
{
    public StudentRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public Task<IEnumerable<StudentModel>> FindStudentsByCourseId(int courseId)
    {
        throw new NotImplementedException();
    }
}

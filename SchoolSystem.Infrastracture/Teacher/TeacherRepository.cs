using SchoolSystem.Infrastracture.Common.BaseClasses;
using SchoolSystem.Infrastracture.Common;
using SchoolSystem.Core.Teacher;

namespace SchoolSystem.Infrastracture.Teacher;

public class TeacherRepository : BaseRepository<TeacherModel>, ITeacherRepository
{
    public TeacherRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}

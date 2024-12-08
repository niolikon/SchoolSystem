using SchoolSystem.Core.Base.BaseInterfaces;

namespace SchoolSystem.Core.Student
{
    public interface IStudentRepository : IBaseRepository<StudentModel>
    {
        Task<IEnumerable<StudentModel>> FindStudentsByCourseId(int courseId);
    }
}

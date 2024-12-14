using SchoolSystem.Core.Common.BaseInterfaces;


namespace SchoolSystem.Core.Student;

public interface IStudentRepository : IBaseRepository<StudentModel, int>
{
    Task<IEnumerable<StudentModel>> FindStudentsByCourseId(int courseId);
}

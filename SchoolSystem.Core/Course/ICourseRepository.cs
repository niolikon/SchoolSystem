using SchoolSystem.Core.Common.BaseInterfaces;

namespace SchoolSystem.Core.Course;

public interface ICourseRepository : IBaseRepository<CourseModel>
{
    Task<IEnumerable<CourseModel>> FindCoursesByStudentId(int studentId);
    Task<IEnumerable<CourseModel>> FindCoursesByTeacherId(int teacherId);
}

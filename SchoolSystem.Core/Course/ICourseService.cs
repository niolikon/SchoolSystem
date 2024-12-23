using SchoolSystem.Core.Common.BaseInterfaces;
using SchoolSystem.Core.Student;


namespace SchoolSystem.Core.Course;

public interface ICourseService : IBaseService<CourseDto>
{
    Task<CourseDto> AddStudentEnrollmentToCourse(int courseId, StudentDto student);

    Task<CourseDto> DeleteStudentEnrollmentToCourse(int courseId, int studentId);

    Task<IEnumerable<StudentDto>> ListStudentsEnrolledToCourse(int courseId);
}

using SchoolSystem.Core.Common.BaseInterfaces;
using SchoolSystem.Core.Student;

namespace SchoolSystem.Core.Course;

public interface ICourseService : IBaseService<CourseDetailsDto, CourseCreateDto, CourseUpdateDto>
{
    Task<CourseDetailsDto> AddStudentEnrollmentToCourse(int courseId, StudentUpdateDto student);

    Task<CourseDetailsDto> DeleteStudentEnrollmentToCourse(int courseId, int studentId);

    Task<IEnumerable<StudentDetailsDto>> ListStudentsEnrolledToCourse(int courseId);
}

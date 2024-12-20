using SchoolSystem.Core.Common.BaseInterfaces;
using SchoolSystem.Core.Student;


namespace SchoolSystem.Core.Course;

public interface ICourseService : IBaseService<CourseDto>
{
    Task<CourseDto> EnrollStudentToCourse(StudentDto student, int courseId);
    Task<CourseDto> DisenrollStudentToCourse(StudentDto student, int courseId);
}

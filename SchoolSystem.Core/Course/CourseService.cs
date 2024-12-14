using SchoolSystem.Core.Common.BaseInterfaces;
using SchoolSystem.Core.Common;
using SchoolSystem.Core.Student;
using SchoolSystem.Core.Teacher;
using System.Collections.Generic;


namespace SchoolSystem.Core.Course;

public class CourseService(
    IBaseMapper<CourseModel, CourseDto> courseDtoMapper,
    IBaseMapper<CourseDto, CourseModel> courseModelMapper,
    ICourseRepository courseRepository,
    IBaseMapper<StudentModel, StudentDto> studentDtoMapper,
    IStudentRepository studentRepository,
    IBaseMapper<TeacherModel, TeacherDto> teacherDtoMapper,
    ITeacherRepository teacherRepository) : ICourseService
{
    public async Task<IEnumerable<CourseDto>> GetAll()
    {
        return courseDtoMapper.MapList(await courseRepository.GetAll());
    }

    public async Task<PaginatedData<CourseDto>> GetAllPaginated(int pageNumber, int pageSize)
    {
        PaginatedData<CourseModel> paginatedCourses = await courseRepository.GetPaginatedData(pageNumber, pageSize);
        List<CourseDto> courses = courseDtoMapper.MapList(paginatedCourses.Data).ToList();

        return new PaginatedData<CourseDto>(courses, paginatedCourses.TotalCount);
    }

    public async Task<CourseDto> GetSingle(int id)
    {
        CourseDto course = courseDtoMapper.MapInstance(await courseRepository.GetById(id));
        IEnumerable<StudentModel> studentsEnrolled = await studentRepository.FindStudentsByCourseId(id);
        course.Students = studentDtoMapper.MapList(studentsEnrolled).ToList();
        //course.Students.ForEach(s => s.Courses = null);
        TeacherModel teacher = await teacherRepository.GetById(course.TeacherId);
        course.Teacher = teacherDtoMapper.MapInstance(teacher);
        //course.Teacher.Courses = null;

        return course;
    }

    public async Task<CourseDto> Create(CourseDto dto)
    {
        CourseModel model = courseModelMapper.MapInstance(dto);
        model.EntryDate = DateTime.Now;

        return courseDtoMapper.MapInstance(await courseRepository.Create(model));
    }

    public async Task Update(int id, CourseDto updateInputDto)
    {
        CourseModel existingCourse = await courseRepository.GetById(id);
        CourseModel updateInputCourse = courseModelMapper.MapInstance(updateInputDto);

        existingCourse.Name = updateInputCourse.Name;
        existingCourse.Credits = updateInputCourse.Credits;
        existingCourse.UpdateDate = DateTime.Now;

        await courseRepository.Update(existingCourse);
    }

    public async Task Delete(int id)
    {
        CourseModel course = await courseRepository.GetById(id);
        await courseRepository.Delete(course);
    }
}

using SchoolSystem.Core.Common;
using SchoolSystem.Core.Common.BaseInterfaces;
using SchoolSystem.Core.Course;


namespace SchoolSystem.Core.Teacher;

public class TeacherService(
    IBaseMapper<TeacherModel, TeacherDto> teacherDtoMapper,
    IBaseMapper<TeacherDto, TeacherModel> teacherModelMapper,
    ITeacherRepository teacherRepository,
    IBaseMapper<CourseModel, CourseDto> courseDtoMapper,
    ICourseRepository courseRepository) : ITeacherService
{
    public async Task<IEnumerable<TeacherDto>> GetAll()
    {
        return teacherDtoMapper.MapList(await teacherRepository.GetAll());
    }

    public async Task<PaginatedData<TeacherDto>> GetAllPaginated(int pageNumber, int pageSize)
    {
        PaginatedData<TeacherModel> paginatedTeachers = await teacherRepository.GetPaginatedData(pageNumber, pageSize);
        List<TeacherDto> teachers = teacherDtoMapper.MapList(paginatedTeachers.Data).ToList();

        return new PaginatedData<TeacherDto> (teachers, paginatedTeachers.TotalCount);
    }

    public async Task<TeacherDto> GetSingle(int id)
    {
        TeacherDto teacher = teacherDtoMapper.MapInstance(await teacherRepository.GetById(id));
        IEnumerable<CourseModel> coursesTeached = await courseRepository.FindCoursesByTeacherId(id);
        teacher.Courses = courseDtoMapper.MapList(coursesTeached).ToList();

        return teacher;
    }

    public async Task<TeacherDto> Create(TeacherDto dto)
    {
        TeacherModel model = teacherModelMapper.MapInstance(dto);
        model.EntryDate = DateTime.Now;

        return teacherDtoMapper.MapInstance(await teacherRepository.Create(model));
    }

    public async Task Update(int id, TeacherDto updateInputDto)
    {
        TeacherModel existingTeacher = await teacherRepository.GetById(id);
        TeacherModel updateInputTeacher = teacherModelMapper.MapInstance(updateInputDto);

        existingTeacher.FullName = updateInputTeacher.FullName;
        existingTeacher.Position = updateInputTeacher.Position;
        existingTeacher.Email = updateInputTeacher.Email;
        existingTeacher.UpdateDate = DateTime.Now;

        await teacherRepository.Update(existingTeacher);
    }

    public async Task Delete(int id)
    {
        TeacherModel teacher = await teacherRepository.GetById(id);
        await teacherRepository.Delete(teacher);
    }
}

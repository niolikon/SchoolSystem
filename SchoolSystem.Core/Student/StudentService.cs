using SchoolSystem.Core.Common;
using SchoolSystem.Core.Common.BaseInterfaces;
using SchoolSystem.Core.Course;


namespace SchoolSystem.Core.Student;

public class StudentService(
    IBaseMapper<StudentModel, StudentDto> studentDtoMapper,
    IBaseMapper<StudentDto, StudentModel> studentModelMapper,
    IStudentRepository studentRepository,
    IBaseMapper<CourseModel, CourseDto> courseDtoMapper,
    ICourseRepository courseRepository) : IStudentService
{
    public async Task<IEnumerable<StudentDto>> GetAll()
    {
        return studentDtoMapper.MapList(await studentRepository.GetAll());
    }

    public async Task<PaginatedData<StudentDto>> GetAllPaginated(int pageNumber, int pageSize)
    {
        PaginatedData<StudentModel> paginatedStudents = await studentRepository.GetPaginatedData(pageNumber, pageSize);
        List<StudentDto> students = studentDtoMapper.MapList(paginatedStudents.Data).ToList();

        return new PaginatedData<StudentDto>(students, paginatedStudents.TotalCount);
    }

    public async Task<StudentDto> GetSingle(int id)
    {
        StudentDto student = studentDtoMapper.MapInstance(await studentRepository.GetById(id));
        IEnumerable<CourseModel> coursesEnrolled = await courseRepository.FindCoursesByStudentId(id);
        student.EnrolledCourses = courseDtoMapper.MapList(coursesEnrolled).ToList();

        return student;
    }

    public async Task<bool> IsExists(string key, string value)
    {
        return await studentRepository.IsExists(key, value);
    }

    public async Task<bool> IsExistsForUpdate(int id, string key, string value)
    {
        return await studentRepository.IsExistsForUpdate(id, key, value);
    }

    public async Task<StudentDto> Create(StudentDto dto)
    {
        StudentModel model = studentModelMapper.MapInstance(dto);
        model.EntryDate = DateTime.Now;

        return studentDtoMapper.MapInstance(await studentRepository.Create(model));
    }

    public async Task Update(StudentDto updateInputDto)
    {
        StudentModel existingStudent = await studentRepository.GetById(updateInputDto.Id);
        StudentModel updateInputStudent = studentModelMapper.MapInstance(updateInputDto);

        existingStudent.FullName = updateInputStudent.FullName;
        existingStudent.Email = updateInputStudent.Email;
        existingStudent.UpdateDate = DateTime.Now;

        await studentRepository.Update(existingStudent);
    }

    public async Task Delete(int id)
    {
        StudentModel student = await studentRepository.GetById(id);
        await studentRepository.Delete(student);
    }
}

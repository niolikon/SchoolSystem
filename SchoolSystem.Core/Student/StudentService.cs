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
        student.Courses = courseDtoMapper.MapList(coursesEnrolled).ToList();

        return student;
    }

    public async Task<StudentDto> Create(StudentDto dto)
    {
        StudentModel model = studentModelMapper.MapInstance(dto);
        model.EntryDate = DateTime.Now;

        return studentDtoMapper.MapInstance(await studentRepository.Create(model));
    }

    public async Task Update(int id, StudentDto updateInputDto)
    {
        StudentModel existingStudent = await studentRepository.GetById(id);
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

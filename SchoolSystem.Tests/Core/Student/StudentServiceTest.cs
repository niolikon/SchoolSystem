using FluentAssertions;
using Moq;
using SchoolSystem.Core.Common.BaseInterfaces;
using SchoolSystem.Core.Course;
using SchoolSystem.Core.Student;
using SchoolSystem.Tests.Core.Course;

namespace SchoolSystem.Tests.Core.Student;


public class StudentServiceTests
{
    private Mock<IBaseMapper<StudentModel, StudentDto>> _studentDtoMapperMock;
    private Mock<IBaseMapper<StudentDto, StudentModel>> _studentModelMapperMock;
    private Mock<IStudentRepository> _studentRepositoryMock;
    private Mock<IBaseMapper<CourseModel, CourseDto>> _courseDtoMapperMock;
    private Mock<ICourseRepository> _courseRepositoryMock;
    private StudentService studentService;

    public StudentServiceTests()
    {
        _studentDtoMapperMock = new Mock<IBaseMapper<StudentModel, StudentDto>>();
        _studentModelMapperMock = new Mock<IBaseMapper<StudentDto, StudentModel>>();
        _studentRepositoryMock = new Mock<IStudentRepository>();
        _courseDtoMapperMock = new Mock<IBaseMapper<CourseModel, CourseDto>>();
        _courseRepositoryMock = new Mock<ICourseRepository>();

        studentService = new StudentService(
            _studentDtoMapperMock.Object,
            _studentModelMapperMock.Object,
            _studentRepositoryMock.Object,
            _courseDtoMapperMock.Object,
            _courseRepositoryMock.Object);
    }

    [Fact]
    public async Task Should_Return_All_On_Get_All()
    {
        IEnumerable<StudentModel> studentsInDb = new List<StudentModel>
        {
                StudentTestData.STUDENT_MODEL_1,
                StudentTestData.STUDENT_MODEL_2
        };
        List<StudentDto> studentsAsDto = new List<StudentDto>
        {
            StudentTestData.STUDENT_DTO_1,
            StudentTestData.STUDENT_DTO_2
        };

        _studentRepositoryMock.Setup(repo => repo.GetAllWithDetails())
            .ReturnsAsync(studentsInDb);

        _studentDtoMapperMock.Setup(mapper => mapper.MapList(studentsInDb))
            .Returns(studentsAsDto);

        IEnumerable<StudentDto> studentsReturned = await studentService.GetAll();

        studentsReturned.Should().BeEquivalentTo(studentsAsDto);
    }

    /*
    [Fact]
    public async Task Should_Return_Correct_Data_On_GetSingle()
    {
        var studentModel = StudentTestData.STUDENT_MODEL_2;
        var studentAsDto = StudentTestData.STUDENT_DTO_2;
        var coursesEnrolledModel = new List<CourseModel>
        {
            CourseTestData.COURSE_MODEL_2_ALGEBRA,
            CourseTestData.COURSE_MODEL_3_STATISTICS
        };
        var coursesEnrolledAsDto = new List<CourseDto>
        {
            CourseTestData.COURSE_DTO_2_ALGEBRA,
            CourseTestData.COURSE_DTO_3_STATISTICS
        };

        _studentRepositoryMock
            .Setup(repo => repo.GetById(studentModel.Id))
            .ReturnsAsync(studentModel);

        _studentDtoMapperMock
            .Setup(mapper => mapper.MapInstance(studentModel))
            .Returns(studentAsDto);

        _courseRepositoryMock
            .Setup(repo => repo.FindCoursesByStudentId(studentModel.Id))
            .ReturnsAsync(coursesEnrolledModel);

        _courseDtoMapperMock
            .Setup(mapper => mapper.MapList(coursesEnrolledModel))
            .Returns(coursesEnrolledAsDto);

        StudentDto studentReturned = await studentService.GetSingle(studentModel.Id);

        studentReturned.Should().Be(studentAsDto);
        studentReturned.Courses.Should().BeEquivalentTo(coursesEnrolledAsDto);
    }*/

    [Fact]
    public async Task Should_Return_Created_Data_On_Create()
    {
        var studentDto = StudentTestData.STUDENT_DTO_2;
        var studentAsModel = StudentTestData.STUDENT_MODEL_2;
        StudentDto studentDtoCreated = new()
        {
            Id = 2345678,
            FullName = studentDto.FullName,
            Email = studentDto.Email,
        };

        _studentModelMapperMock
            .Setup(mapper => mapper.MapInstance(studentDto))
            .Returns(studentAsModel);

        _studentRepositoryMock
            .Setup(repo => repo.Create(It.IsAny<StudentModel>()))
            .ReturnsAsync(studentAsModel);

        _studentDtoMapperMock
            .Setup(mapper => mapper.MapInstance(studentAsModel))
            .Returns(studentDtoCreated);

        StudentDto studentReturned = await studentService.Create(studentDto);

        studentReturned.FullName.Should().Be(studentDtoCreated.FullName);
        studentReturned.Email.Should().Be(studentDtoCreated.Email);
    }
}

using FluentAssertions;
using Moq;
using SchoolSystem.Core.Common.BaseInterfaces;
using SchoolSystem.Core.Course;
using SchoolSystem.Core.Student;
using SchoolSystem.Core.Teacher;
using SchoolSystem.Tests.Core.Student;
using SchoolSystem.Tests.Core.Teacher;

namespace SchoolSystem.Tests.Core.Course;


public class CourseServiceTests
{
    private readonly Mock<IBaseMapper<CourseModel, CourseDto>> _courseDtoMapperMock;
    private readonly Mock<IBaseMapper<CourseDto, CourseModel>> _courseModelMapperMock;
    private readonly Mock<ICourseRepository> _courseRepositoryMock;
    private readonly Mock<IBaseMapper<StudentModel, StudentDto>> _studentDtoMapperMock;
    private readonly Mock<IStudentRepository> _studentRepositoryMock;
    private readonly Mock<IBaseMapper<TeacherModel, TeacherDto>> _teacherDtoMapperMock;
    private readonly Mock<ITeacherRepository> _teacherRepositoryMock;
    private readonly CourseService _courseService;


    public CourseServiceTests()
    {
        _courseDtoMapperMock = new Mock<IBaseMapper<CourseModel, CourseDto>>();
        _courseModelMapperMock = new Mock<IBaseMapper<CourseDto, CourseModel>>();
        _courseRepositoryMock = new Mock<ICourseRepository>();
        _studentDtoMapperMock = new Mock<IBaseMapper<StudentModel, StudentDto>>();
        _studentRepositoryMock = new Mock<IStudentRepository>();
        _teacherDtoMapperMock = new Mock<IBaseMapper<TeacherModel, TeacherDto>>();
        _teacherRepositoryMock = new Mock<ITeacherRepository>();

        _courseService = new CourseService(
            _courseDtoMapperMock.Object,
            _courseModelMapperMock.Object,
            _courseRepositoryMock.Object,
            _studentDtoMapperMock.Object,
            _studentRepositoryMock.Object,
            _teacherDtoMapperMock.Object,
            _teacherRepositoryMock.Object);
    }

    [Fact]
    public async Task Should_Return_All_On_Get_All()
    {
        IEnumerable<CourseModel> coursesInDb = new List<CourseModel>
        {
                CourseTestData.COURSE_MODEL_2_ALGEBRA,
                CourseTestData.COURSE_MODEL_3_STATISTICS,
                CourseTestData.COURSE_MODEL_1_CALCULUS
        };
        List<CourseDto> coursesAsDto = new List<CourseDto>
        {
                CourseTestData.COURSE_DTO_2_ALGEBRA,
                CourseTestData.COURSE_DTO_3_STATISTICS,
                CourseTestData.COURSE_DTO_1_CALCULUS
        };

        _courseRepositoryMock.Setup(repo => repo.GetAllWithDetails())
            .ReturnsAsync(coursesInDb.AsQueryable());

        _courseDtoMapperMock.Setup(mapper => mapper.MapList(coursesInDb))
            .Returns(coursesAsDto);

        IEnumerable<CourseDto> coursesReturned = await _courseService.GetAll();

        coursesReturned.Should().BeEquivalentTo(coursesAsDto);
    }

    [Fact]
    public async Task Should_Return_Created_Data_On_Create()
    {
        var courseDto = CourseTestData.COURSE_DTO_3_STATISTICS;
        var courseAsModel = CourseTestData.COURSE_MODEL_3_STATISTICS;
        CourseDto courseDtoCreated = new()
        {
            Id = 2345678,
            Name = courseDto.Name,
            Credits = courseDto.Credits,
            TeacherId = courseDto.TeacherId
        };

        _courseModelMapperMock
            .Setup(mapper => mapper.MapInstance(courseDto))
            .Returns(courseAsModel);

        _courseRepositoryMock
            .Setup(repo => repo.Create(It.IsAny<CourseModel>()))
            .ReturnsAsync(courseAsModel);

        _courseDtoMapperMock
            .Setup(mapper => mapper.MapInstance(courseAsModel))
            .Returns(courseDtoCreated);

        CourseDto courseReturned = await _courseService.Create(courseDto);

        courseReturned.Name.Should().Be(courseDtoCreated.Name);
        courseReturned.Credits.Should().Be(courseDtoCreated.Credits);
        courseReturned.TeacherId.Should().Be(courseDtoCreated.TeacherId);
    }
}

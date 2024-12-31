using FluentAssertions;
using Moq;
using SchoolSystem.Core.Common.BaseInterfaces;
using SchoolSystem.Core.Course;
using SchoolSystem.Core.Student;

namespace SchoolSystem.Tests.Core.Course;

public class CourseServiceTests
{
    private readonly Mock<IBaseMapper<CourseModel, CourseDetailsDto>> _courseDtoMapperMock;
    private readonly Mock<IBaseMapper<CourseCreateDto, CourseModel>> _courseCreateToModelMapper;
    private readonly Mock<IBaseMapper<CourseUpdateDto, CourseModel>> _courseUpdateToModelMapper;
    private readonly Mock<ICourseRepository> _courseRepositoryMock;
    private readonly Mock<IStudentRepository> _studentRepositoryMock;
    private readonly CourseService _courseService;


    public CourseServiceTests()
    {
        _courseDtoMapperMock = new Mock<IBaseMapper<CourseModel, CourseDetailsDto>>();
        _courseCreateToModelMapper = new Mock<IBaseMapper<CourseCreateDto, CourseModel>>();
        _courseUpdateToModelMapper = new Mock<IBaseMapper<CourseUpdateDto, CourseModel>>();
        _courseRepositoryMock = new Mock<ICourseRepository>();
        _studentRepositoryMock = new Mock<IStudentRepository>();

        _courseService = new CourseService(
            _courseDtoMapperMock.Object,
            _courseCreateToModelMapper.Object,
            _courseUpdateToModelMapper.Object,
            _courseRepositoryMock.Object,
            _studentRepositoryMock.Object);
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
        List<CourseDetailsDto> coursesAsDto = new List<CourseDetailsDto>
        {
                CourseTestData.COURSE_DETAILS_2_ALGEBRA,
                CourseTestData.COURSE_DETAILS_3_STATISTICS,
                CourseTestData.COURSE_DETAILS_1_CALCULUS
        };

        _courseRepositoryMock.Setup(repo => repo.GetAllWithDetails())
            .ReturnsAsync(coursesInDb.AsQueryable());

        _courseDtoMapperMock.Setup(mapper => mapper.MapList(coursesInDb))
            .Returns(coursesAsDto);

        IEnumerable<CourseDetailsDto> coursesReturned = await _courseService.GetAll();

        coursesReturned.Should().BeEquivalentTo(coursesAsDto);
    }

    [Fact]
    public async Task Should_Return_Created_Data_On_Create()
    {
        CourseCreateDto courseDto = new() 
        { 
            Name = "Test", 
            Credits = 10,
            TeacherId = 1
        };
        CourseModel courseAsModel = new()
        {
            Name = courseDto.Name,
            Credits = courseDto.Credits,
            TeacherId = courseDto.TeacherId
        };
        CourseDetailsDto courseDtoResulting = new()
        {
            Id = 2345678,
            Name = courseAsModel.Name,
            Credits = courseAsModel.Credits,
            TeacherId = courseAsModel.TeacherId
        };

        _courseCreateToModelMapper
            .Setup(mapper => mapper.MapInstance(courseDto))
            .Returns(courseAsModel);

        _courseRepositoryMock
            .Setup(repo => repo.Create(It.IsAny<CourseModel>()))
            .ReturnsAsync(courseAsModel);

        _courseDtoMapperMock
            .Setup(mapper => mapper.MapInstance(courseAsModel))
            .Returns(courseDtoResulting);

        CourseDetailsDto courseReturned = await _courseService.Create(courseDto);

        courseReturned.Name.Should().Be(courseDto.Name);
        courseReturned.Credits.Should().Be(courseDto.Credits);
        courseReturned.TeacherId.Should().Be(courseDto.TeacherId);
    }
}

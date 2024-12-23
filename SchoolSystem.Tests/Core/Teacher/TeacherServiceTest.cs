using FluentAssertions;
using Moq;
using SchoolSystem.Core.Teacher;
using SchoolSystem.Core.Course;
using SchoolSystem.Core.Common.BaseInterfaces;
using SchoolSystem.Tests.Core.Course;

namespace SchoolSystem.Tests.Core.Teacher;


public class TeacherServiceTests
{
    private Mock<IBaseMapper<TeacherModel, TeacherDto>> _teacherDtoMapperMock;
    private Mock<IBaseMapper<TeacherDto, TeacherModel>> _teacherModelMapperMock;
    private Mock<ITeacherRepository> _teacherRepositoryMock;
    private Mock<IBaseMapper<CourseModel, CourseDto>> _courseDtoMapperMock;
    private Mock<ICourseRepository> _courseRepositoryMock;
    private TeacherService teacherService;


    public TeacherServiceTests()
    {
        _teacherDtoMapperMock = new Mock<IBaseMapper<TeacherModel, TeacherDto>>();
        _teacherModelMapperMock = new Mock<IBaseMapper<TeacherDto, TeacherModel>>();
        _teacherRepositoryMock = new Mock<ITeacherRepository>();
        _courseDtoMapperMock = new Mock<IBaseMapper<CourseModel, CourseDto>>();
        _courseRepositoryMock = new Mock<ICourseRepository>();

        teacherService = new TeacherService(
            _teacherDtoMapperMock.Object,
            _teacherModelMapperMock.Object,
            _teacherRepositoryMock.Object,
            _courseDtoMapperMock.Object,
            _courseRepositoryMock.Object);
    }

    [Fact]
    public async Task Should_Return_All_On_Get_All()
    {
        IEnumerable<TeacherModel> teachersInDb = new List<TeacherModel> 
        {
                TeacherTestData.TEACHER_MODEL_1_ASSISTANT,
                TeacherTestData.TEACHER_MODEL_2_ASSOCIATED
        };
        List<TeacherDto> teachersAsDto = new List<TeacherDto>
        {
            TeacherTestData.TEACHER_DTO_1_ASSISTANT,
            TeacherTestData.TEACHER_DTO_2_ASSOCIATED
        };

        _teacherRepositoryMock.Setup(repo => repo.GetAllWithDetails())
            .ReturnsAsync(teachersInDb);

        _teacherDtoMapperMock.Setup(mapper => mapper.MapList(teachersInDb))
            .Returns(teachersAsDto);

        IEnumerable<TeacherDto> teachersReturned = await teacherService.GetAll();

        teachersReturned.Should().BeEquivalentTo(teachersAsDto);
    }

    /*
    [Fact]
    public async Task Should_Return_Correct_Data_On_GetSingle()
    {
        var teacherModel = TeacherTestData.TEACHER_MODEL_1_ASSISTANT;
        var teacherAsDto = TeacherTestData.TEACHER_DTO_1_ASSISTANT;
        var coursesTeachedModel = new List<CourseModel>
        {
            CourseTestData.COURSE_MODEL_1_CALCULUS,
            CourseTestData.COURSE_MODEL_3_STATISTICS
        };
        var coursesTeachedAsDto = new List<CourseDto> 
        {
            CourseTestData.COURSE_DTO_1_CALCULUS,
            CourseTestData.COURSE_DTO_3_STATISTICS
        };

        _teacherRepositoryMock
            .Setup(repo => repo.GetById(teacherModel.Id))
            .ReturnsAsync(teacherModel);

        _teacherDtoMapperMock
            .Setup(mapper => mapper.MapInstance(teacherModel))
            .Returns(teacherAsDto);

        _courseRepositoryMock
            .Setup(repo => repo.FindCoursesByTeacherId(teacherModel.Id))
            .ReturnsAsync(coursesTeachedModel);

        _courseDtoMapperMock
            .Setup(mapper => mapper.MapList(coursesTeachedModel))
            .Returns(coursesTeachedAsDto);

        TeacherDto teacherReturned = await teacherService.GetSingle(teacherModel.Id);

        teacherReturned.Should().Be(teacherAsDto);
        teacherReturned.Courses.Should().BeEquivalentTo(coursesTeachedAsDto);
    }*/

    [Fact]
    public async Task Should_Return_Created_Data_On_Create()
    {
        var teacherDto = TeacherTestData.TEACHER_DTO_1_ASSISTANT;
        var teacherAsModel = TeacherTestData.TEACHER_MODEL_1_ASSISTANT;
        TeacherDto teacherDtoCreated = new () {
            Id = 1234567,
            FullName = teacherDto.FullName,
            Position = teacherDto.Position,
            Email = teacherDto.Email,
        };
        
        _teacherModelMapperMock
            .Setup(mapper => mapper.MapInstance(teacherDto))
            .Returns(teacherAsModel);

        _teacherRepositoryMock
            .Setup(repo => repo.Create(It.IsAny<TeacherModel>()))
            .ReturnsAsync(teacherAsModel);

        _teacherDtoMapperMock
            .Setup(mapper => mapper.MapInstance(teacherAsModel))
            .Returns(teacherDtoCreated);

        TeacherDto teacherReturned = await teacherService.Create(teacherDto);

        teacherReturned.FullName.Should().Be(teacherDtoCreated.FullName);
        teacherReturned.Position.Should().Be(teacherDtoCreated.Position);
        teacherReturned.Email.Should().Be(teacherDtoCreated.Email);
    }
}

using Moq;
using SchoolSystem.Core.Teacher;
using SchoolSystem.Core.Course;
using SchoolSystem.Core.Common.BaseInterfaces;
using SchoolSystem.Tests.Core.Course;


namespace SchoolSystem.Tests.Core.Teacher;

[TestFixture]
public class TeacherServiceTests
{
    private Mock<IBaseMapper<TeacherModel, TeacherDto>> _teacherDtoMapperMock;
    private Mock<IBaseMapper<TeacherDto, TeacherModel>> _teacherModelMapperMock;
    private Mock<ITeacherRepository> _teacherRepositoryMock;
    private Mock<IBaseMapper<CourseModel, CourseDto>> _courseDtoMapperMock;
    private Mock<ICourseRepository> _courseRepositoryMock;
    private TeacherService teacherService;

    [SetUp]
    public void Setup()
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

    [Test]
    public async Task GetAll_ReturnAll()
    {
        IEnumerable<TeacherModel> teachersInDb = new List<TeacherModel> 
        {
                TeacherTestData.TEACHER_MODEL_ASSISTANT,
                TeacherTestData.TEACHER_MODEL_ASSOCIATED
        };
        List<TeacherDto> teachersAsDto = new List<TeacherDto>
        {
            TeacherTestData.TEACHER_DTO_ASSISTANT,
            TeacherTestData.TEACHER_DTO_ASSOCIATED
        };

        _teacherRepositoryMock.Setup(repo => repo.GetAll())
            .ReturnsAsync(teachersInDb);

        _teacherDtoMapperMock.Setup(mapper => mapper.MapList(teachersInDb))
            .Returns(teachersAsDto);

        IEnumerable<TeacherDto> teachersReturned = await teacherService.GetAll();

        Assert.That(teachersReturned, Is.EquivalentTo(teachersAsDto));
    }

    [Test]
    public async Task GetSingle_ShouldReturnTeacherWithCourses()
    {
        var teacherModel = TeacherTestData.TEACHER_MODEL_ASSISTANT;
        var teacherAsDto = TeacherTestData.TEACHER_DTO_ASSISTANT;
        var coursesTeachedModel = new List<CourseModel>
        {
            CourseTestData.COURSE_MODEL_CALCULUS,
            CourseTestData.COURSE_MODEL_STATISTICS
        };
        var coursesTeachedAsDto = new List<CourseDto> 
        {
            CourseTestData.COURSE_DTO_CALCULUS,
            CourseTestData.COURSE_DTO_STATISTICS
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

        Assert.Multiple(() =>
        {
            Assert.That(teacherReturned, Is.EqualTo(teacherAsDto));
            Assert.That(teacherReturned.TeachedCourses, Is.EquivalentTo(coursesTeachedAsDto));
        });
    }

    [Test]
    public async Task Create_ShouldReturnCreatedTeacherDto()
    {
        var teacherDto = TeacherTestData.TEACHER_DTO_ASSISTANT;
        var teacherAsModel = TeacherTestData.TEACHER_MODEL_ASSISTANT;
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

        Assert.Multiple(() =>
        {
            Assert.That(teacherReturned.FullName, Is.EqualTo(teacherDtoCreated.FullName));
            Assert.That(teacherReturned.Position, Is.EqualTo(teacherDtoCreated.Position));
            Assert.That(teacherReturned.Email, Is.EqualTo(teacherDtoCreated.Email));
        });
    }
}

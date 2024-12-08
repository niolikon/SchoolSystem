using Moq;
using SchoolSystem.Core.Common.BaseInterfaces;
using SchoolSystem.Core.Course;
using SchoolSystem.Core.Student;
using SchoolSystem.Core.Teacher;
using SchoolSystem.Tests.Core.Student;
using SchoolSystem.Tests.Core.Teacher;


namespace SchoolSystem.Tests.Core.Course;

[TestFixture]
public class CourseServiceTests
{
    private Mock<IBaseMapper<CourseModel, CourseDto>> _courseDtoMapperMock;
    private Mock<IBaseMapper<CourseDto, CourseModel>> _courseModelMapperMock;
    private Mock<ICourseRepository> _courseRepositoryMock;
    private Mock<IBaseMapper<StudentModel, StudentDto>> _studentDtoMapperMock;
    private Mock<IStudentRepository> _studentRepositoryMock;
    private Mock<IBaseMapper<TeacherModel, TeacherDto>> _teacherDtoMapperMock;
    private Mock<ITeacherRepository> _teacherRepositoryMock;
    private CourseService courseService;

    [SetUp]
    public void Setup()
    {
        _courseDtoMapperMock = new Mock<IBaseMapper<CourseModel, CourseDto>>();
        _courseModelMapperMock = new Mock<IBaseMapper<CourseDto, CourseModel>>();
        _courseRepositoryMock = new Mock<ICourseRepository>();
        _studentDtoMapperMock = new Mock<IBaseMapper<StudentModel, StudentDto>>();
        _studentRepositoryMock = new Mock<IStudentRepository>();
        _teacherDtoMapperMock = new Mock<IBaseMapper<TeacherModel, TeacherDto>>();
        _teacherRepositoryMock = new Mock<ITeacherRepository>();

        courseService = new CourseService(
            _courseDtoMapperMock.Object,
            _courseModelMapperMock.Object,
            _courseRepositoryMock.Object,
            _studentDtoMapperMock.Object,
            _studentRepositoryMock.Object,
            _teacherDtoMapperMock.Object,
            _teacherRepositoryMock.Object);
    }

    [Test]
    public async Task GetAll_ReturnAll()
    {
        IEnumerable<CourseModel> coursesInDb = new List<CourseModel>
        {
                CourseTestData.COURSE_MODEL_ALGEBRA,
                CourseTestData.COURSE_MODEL_STATISTICS,
                CourseTestData.COURSE_MODEL_CALCULUS
        };
        List<CourseDto> coursesAsDto = new List<CourseDto>
        {
                CourseTestData.COURSE_DTO_ALGEBRA,
                CourseTestData.COURSE_DTO_STATISTICS,
                CourseTestData.COURSE_DTO_CALCULUS
        };

        _courseRepositoryMock.Setup(repo => repo.GetAll())
            .ReturnsAsync(coursesInDb);

        _courseDtoMapperMock.Setup(mapper => mapper.MapList(coursesInDb))
            .Returns(coursesAsDto);

        IEnumerable<CourseDto> coursesReturned = await courseService.GetAll();

        Assert.That(coursesReturned, Is.EquivalentTo(coursesAsDto));
    }

    [Test]
    public async Task GetSingle_ShouldReturnCourseWithStudentsAndTeacher()
    {
        var courseModel = CourseTestData.COURSE_MODEL_CALCULUS;
        var courseAsDto = CourseTestData.COURSE_DTO_CALCULUS;
        var studentsEnrolledModel = new List<StudentModel>
        {
            StudentTestData.STUDENT_MODEL_1,
            StudentTestData.STUDENT_MODEL_2
        };
        var studentsEnrolledAsDto = new List<StudentDto>
        {
            StudentTestData.STUDENT_DTO_1,
            StudentTestData.STUDENT_DTO_2
        };
        var teacherModel = TeacherTestData.TEACHER_MODEL_ASSOCIATED;
        var teacherAsDto = TeacherTestData.TEACHER_DTO_ASSOCIATED;

        _courseRepositoryMock
            .Setup(repo => repo.GetById(courseModel.Id))
            .ReturnsAsync(courseModel);

        _courseDtoMapperMock
            .Setup(mapper => mapper.MapInstance(courseModel))
            .Returns(courseAsDto);

        _studentRepositoryMock
            .Setup(repo => repo.FindStudentsByCourseId(courseModel.Id))
            .ReturnsAsync(studentsEnrolledModel);

        _studentDtoMapperMock
            .Setup(mapper => mapper.MapList(studentsEnrolledModel))
            .Returns(studentsEnrolledAsDto);

        _teacherRepositoryMock
            .Setup(repo => repo.GetById(courseModel.TeacherId))
            .ReturnsAsync(teacherModel);

        _teacherDtoMapperMock
            .Setup(mapper => mapper.MapInstance(teacherModel))
            .Returns(teacherAsDto);

        CourseDto courseReturned = await courseService.GetSingle(courseModel.Id);

        Assert.Multiple(() =>
        {
            Assert.That(courseReturned, Is.EqualTo(courseAsDto));
            Assert.That(courseReturned.EnrolledStudents, Is.EquivalentTo(studentsEnrolledAsDto));
            Assert.That(courseReturned.Teacher, Is.EqualTo(teacherAsDto));
        });
    }

    [Test]
    public async Task Create_ShouldReturnCreatedCourseDto()
    {
        var courseDto = CourseTestData.COURSE_DTO_STATISTICS;
        var courseAsModel = CourseTestData.COURSE_MODEL_STATISTICS;
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

        CourseDto courseReturned = await courseService.Create(courseDto);

        Assert.Multiple(() =>
        {
            Assert.That(courseReturned.Name, Is.EqualTo(courseDtoCreated.Name));
            Assert.That(courseReturned.Credits, Is.EqualTo(courseDtoCreated.Credits));
            Assert.That(courseReturned.TeacherId, Is.EqualTo(courseDtoCreated.TeacherId));
        });
    }
}

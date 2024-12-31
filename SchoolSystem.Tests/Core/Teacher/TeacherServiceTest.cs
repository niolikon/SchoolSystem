using FluentAssertions;
using Moq;
using SchoolSystem.Core.Teacher;
using SchoolSystem.Core.Common.BaseInterfaces;

namespace SchoolSystem.Tests.Core.Teacher;

public class TeacherServiceTests
{
    private readonly Mock<IBaseMapper<TeacherModel, TeacherDetailsDto>> _teacherDtoMapperMock;
    private readonly Mock<IBaseMapper<TeacherCreateDto, TeacherModel>> _teacherCreateToModelMapperMock;
    private readonly Mock<IBaseMapper<TeacherUpdateDto, TeacherModel>> _teacherUpdateToModelMapperMock;
    private readonly Mock<ITeacherRepository> _teacherRepositoryMock;
    private readonly TeacherService _teacherService;


    public TeacherServiceTests()
    {
        _teacherDtoMapperMock = new Mock<IBaseMapper<TeacherModel, TeacherDetailsDto>>();
        _teacherCreateToModelMapperMock = new Mock<IBaseMapper<TeacherCreateDto, TeacherModel>>();
        _teacherUpdateToModelMapperMock = new Mock<IBaseMapper<TeacherUpdateDto, TeacherModel>>();
        _teacherRepositoryMock = new Mock<ITeacherRepository>();

        _teacherService = new TeacherService(
            _teacherDtoMapperMock.Object,
            _teacherCreateToModelMapperMock.Object,
            _teacherUpdateToModelMapperMock.Object,
            _teacherRepositoryMock.Object);
    }

    [Fact]
    public async Task Should_Return_All_On_Get_All()
    {
        IEnumerable<TeacherModel> teachersInDb = new List<TeacherModel>
        {
            TeacherTestData.TEACHER_MODEL_1_ASSISTANT,
            TeacherTestData.TEACHER_MODEL_2_ASSOCIATED
        };
        List<TeacherDetailsDto> teachersAsDto = new List<TeacherDetailsDto>
        {
            TeacherTestData.TEACHER_DETAILS_1_ASSISTANT,
            TeacherTestData.TEACHER_DETAILS_2_ASSOCIATED
        };

        _teacherRepositoryMock.Setup(repo => repo.GetAllWithDetails())
            .ReturnsAsync(teachersInDb);

        _teacherDtoMapperMock.Setup(mapper => mapper.MapList(teachersInDb))
            .Returns(teachersAsDto);

        IEnumerable<TeacherDetailsDto> teachersReturned = await _teacherService.GetAll();

        teachersReturned.Should().BeEquivalentTo(teachersAsDto);
    }

    [Fact]
    public async Task Should_Return_Created_Data_On_Create()
    {
        TeacherCreateDto teacherDto = new()
        {
            Email = "doc.test@test.it",
            FullName = "Doc Robben Ford",
            Position = AcademicPosition.Instructor.ToString()
        };
        TeacherModel teacherAsModel = new()
        {
            Email = teacherDto.Email,
            FullName = teacherDto.FullName,
            Position = AcademicPosition.Instructor
        };
        TeacherDetailsDto teacherDtoResulting = new() {
            Id = 1234567,
            Email = teacherAsModel.Email,
            FullName = teacherAsModel.FullName,
            Position = teacherAsModel.Position.ToString()
        };
        
        _teacherCreateToModelMapperMock
            .Setup(mapper => mapper.MapInstance(teacherDto))
            .Returns(teacherAsModel);

        _teacherRepositoryMock
            .Setup(repo => repo.Create(It.IsAny<TeacherModel>()))
            .ReturnsAsync(teacherAsModel);

        _teacherDtoMapperMock
            .Setup(mapper => mapper.MapInstance(teacherAsModel))
            .Returns(teacherDtoResulting);

        TeacherDetailsDto teacherReturned = await _teacherService.Create(teacherDto);

        teacherReturned.FullName.Should().Be(teacherDto.FullName);
        teacherReturned.Position.Should().Be(teacherDto.Position);
        teacherReturned.Email.Should().Be(teacherDto.Email);
    }
}

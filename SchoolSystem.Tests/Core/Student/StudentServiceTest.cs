using FluentAssertions;
using Moq;
using SchoolSystem.Core.Student;
using SchoolSystem.Core.Common.BaseInterfaces;

namespace SchoolSystem.Tests.Core.Student;

public class StudentServiceTests
{
    private readonly Mock<IBaseMapper<StudentModel, StudentDetailsDto>> _studentDtoMapperMock;
    private readonly Mock<IBaseMapper<StudentCreateDto, StudentModel>> _studentCreateToModelMapperMock;
    private readonly Mock<IBaseMapper<StudentUpdateDto, StudentModel>> _studentUpdateToModelMapperMock;
    private readonly Mock<IStudentRepository> _studentRepositoryMock;
    private readonly StudentService _studentService;

    public StudentServiceTests()
    {
        _studentDtoMapperMock = new Mock<IBaseMapper<StudentModel, StudentDetailsDto>>();
        _studentCreateToModelMapperMock = new Mock<IBaseMapper<StudentCreateDto, StudentModel>>();
        _studentUpdateToModelMapperMock = new Mock<IBaseMapper<StudentUpdateDto, StudentModel>>();
        _studentRepositoryMock = new Mock<IStudentRepository>();

        _studentService = new StudentService(
            _studentDtoMapperMock.Object,
            _studentCreateToModelMapperMock.Object,
            _studentUpdateToModelMapperMock.Object,
            _studentRepositoryMock.Object);
    }

    [Fact]
    public async Task Should_Return_All_On_Get_All()
    {
        IEnumerable<StudentModel> studentsInDb = new List<StudentModel>
        {
                StudentTestData.STUDENT_MODEL_1,
                StudentTestData.STUDENT_MODEL_2
        };
        List<StudentDetailsDto> studentsAsDto = new List<StudentDetailsDto>
        {
            StudentTestData.STUDENT_DETAILS_DTO_1,
            StudentTestData.STUDENT_DETAILS_DTO_2
        };

        _studentRepositoryMock.Setup(repo => repo.GetAllWithDetails())
            .ReturnsAsync(studentsInDb);

        _studentDtoMapperMock.Setup(mapper => mapper.MapList(studentsInDb))
            .Returns(studentsAsDto);

        IEnumerable<StudentDetailsDto> studentsReturned = await _studentService.GetAll();

        studentsReturned.Should().BeEquivalentTo(studentsAsDto);
    }

    [Fact]
    public async Task Should_Return_Created_Data_On_Create()
    {
        StudentCreateDto studentDto = new() 
        { 
            Email = "test@test.it", 
            FullName = "Robben Ford" 
        };
        StudentModel studentAsModel = new() 
        { 
            Email = studentDto.Email, 
            FullName = studentDto.FullName 
        };
        StudentDetailsDto studentDtoResulting = new()
        {
            Id = 2345678,
            Email = studentDto.Email,
            FullName = studentDto.FullName,
        };

        _studentCreateToModelMapperMock
            .Setup(mapper => mapper.MapInstance(studentDto))
            .Returns(studentAsModel);

        _studentRepositoryMock
            .Setup(repo => repo.Create(It.IsAny<StudentModel>()))
            .ReturnsAsync(studentAsModel);

        _studentDtoMapperMock
            .Setup(mapper => mapper.MapInstance(studentAsModel))
            .Returns(studentDtoResulting);

        StudentDetailsDto studentReturned = await _studentService.Create(studentDto);

        studentReturned.FullName.Should().Be(studentDto.FullName);
        studentReturned.Email.Should().Be(studentDto.Email);
    }
}

using FluentAssertions;
using AutoMapper;
using SchoolSystem.Core.Course;
using SchoolSystem.Core.Student;
using SchoolSystem.Core.Common.BaseInterfaces;
using SchoolSystem.Core.Common.BaseClasses;
using SchoolSystem.Core.Common;
using SchoolSystem.Tests.Core.Course;

namespace SchoolSystem.Tests.Core.Student;

public class StudentMapperTest
{
    private readonly IBaseMapper<StudentModel, StudentDetailsDto> _studentDtoMapper;
    private readonly IBaseMapper<StudentCreateDto, StudentModel> _studentCreateToModelMapper;
    private readonly IBaseMapper<StudentUpdateDto, StudentModel> _studentUpdateToModelMapper;

    public StudentMapperTest()
    {
        IMapper _mapper = MapperConfigurationFactory.CreateMapper();
        _studentDtoMapper = new BaseMapper<StudentModel, StudentDetailsDto>(_mapper);
        _studentCreateToModelMapper = new BaseMapper<StudentCreateDto, StudentModel>(_mapper);
        _studentUpdateToModelMapper = new BaseMapper<StudentUpdateDto, StudentModel>(_mapper);
    }

    [Theory]
    [MemberData(nameof(StudentTestData.StudentModelTestCases), MemberType = typeof(StudentTestData))]
    public void Should_Map_Model_To_Dto(StudentModel model)
    {
        List<CourseModel> courses = [CourseTestData.COURSE_MODEL_1_CALCULUS, CourseTestData.COURSE_MODEL_2_ALGEBRA];
        courses.ForEach(c => c.Students = [model]);
        model.Courses = courses;

        var dto = _studentDtoMapper.MapInstance(model);

        dto.Id.Should().Be(model.Id);
        dto.FullName.Should().Be(model.FullName);
        dto.Email.Should().Be(model.Email);
        dto.Courses.Should().AllSatisfy(c => c.Students.Should().BeNull());
    }

    [Theory]
    [MemberData(nameof(StudentTestData.StudentCreateDtoTestCases), MemberType = typeof(StudentTestData))]
    public void Should_Map_Create_Dto_To_Model(StudentCreateDto dto)
    {
        var model = _studentCreateToModelMapper.MapInstance(dto);

        model.FullName.Should().Be(dto.FullName);
        model.Email.Should().Be(dto.Email);
    }

    [Theory]
    [MemberData(nameof(StudentTestData.StudentUpdateDtoTestCases), MemberType = typeof(StudentTestData))]
    public void Should_Map_Update_Dto_To_Model(StudentUpdateDto dto)
    {
        var model = _studentUpdateToModelMapper.MapInstance(dto);

        model.Id.Should().Be(dto.Id);
        model.FullName.Should().Be(dto.FullName);
        model.Email.Should().Be(dto.Email);
    }
}

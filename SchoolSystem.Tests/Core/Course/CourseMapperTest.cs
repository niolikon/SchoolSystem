using FluentAssertions;
using AutoMapper;
using SchoolSystem.Core.Course;
using SchoolSystem.Core.Student;
using SchoolSystem.Core.Teacher;
using SchoolSystem.Core.Common.BaseInterfaces;
using SchoolSystem.Core.Common.BaseClasses;
using SchoolSystem.Core.Common;
using SchoolSystem.Tests.Core.Teacher;
using SchoolSystem.Tests.Core.Student;

namespace SchoolSystem.Tests.Core.Course;

public class CourseMapperTest
{
    private readonly IBaseMapper<CourseModel, CourseDetailsDto> _courseDtoMapper;
    private readonly IBaseMapper<CourseCreateDto, CourseModel> _courseCreateToModelMapper;
    private readonly IBaseMapper<CourseUpdateDto, CourseModel> _courseUpdateToModelMapper;

    public CourseMapperTest()
    {
        IMapper _mapper = MapperConfigurationFactory.CreateMapper();
        _courseDtoMapper = new BaseMapper<CourseModel, CourseDetailsDto>(_mapper);
        _courseCreateToModelMapper = new BaseMapper<CourseCreateDto, CourseModel>(_mapper);
        _courseUpdateToModelMapper = new BaseMapper<CourseUpdateDto, CourseModel>(_mapper);
    }

    [Theory]
    [MemberData(nameof(CourseTestData.CourseModelTestCases), MemberType = typeof(CourseTestData))]
    public void Should_Map_Model_To_Dto(CourseModel model)
    {
        TeacherModel teacher = TeacherTestData.TEACHER_MODEL_2_ASSOCIATED;
        model.Teacher = teacher;
        model.TeacherId = teacher.Id;
        List<StudentModel> students = [StudentTestData.STUDENT_MODEL_1, StudentTestData.STUDENT_MODEL_2];
        model.Students.AddRange(students);

        var dto = _courseDtoMapper.MapInstance(model);

        dto.Id.Should().Be(model.Id);
        dto.Name.Should().Be(model.Name);
        dto.Credits.Should().Be(model.Credits);
        dto.TeacherId.Should().Be(model.TeacherId);
        dto.Teacher?.Courses.Should().BeNull();
        dto.Students.Should().AllSatisfy(s => s.Courses.Should().BeNull());
    }

    [Theory]
    [MemberData(nameof(CourseTestData.CourseCreateDtoTestCases), MemberType = typeof(CourseTestData))]
    public void Should_Map_Create_Dto_To_Model(CourseCreateDto dto)
    {
        var model = _courseCreateToModelMapper.MapInstance(dto);

        model.Name.Should().Be(dto.Name);
        model.Credits.Should().Be(dto.Credits);
        model.TeacherId.Should().Be(dto.TeacherId);
    }

    [Theory]
    [MemberData(nameof(CourseTestData.CourseUpdateDtoTestCases), MemberType = typeof(CourseTestData))]
    public void Should_Map_Update_Dto_To_Model(CourseUpdateDto dto)
    {
        var model = _courseUpdateToModelMapper.MapInstance(dto);

        model.Id.Should().Be(dto.Id);
        model.Name.Should().Be(dto.Name);
        model.Credits.Should().Be(dto.Credits);
        model.TeacherId.Should().Be(dto.TeacherId);
    }
}

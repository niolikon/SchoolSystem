using FluentAssertions;
using AutoMapper;
using SchoolSystem.Core.Course;
using SchoolSystem.Core.Common.BaseInterfaces;
using SchoolSystem.Core.Common.BaseClasses;
using SchoolSystem.Core.Common;
using SchoolSystem.Core.Teacher;
using SchoolSystem.Tests.Core.Teacher;
using SchoolSystem.Tests.Core.Student;
using SchoolSystem.Core.Student;

namespace SchoolSystem.Tests.Core.Course;


public class CourseMapperTest
{
    private readonly IBaseMapper<CourseModel, CourseDto> _courseDtoMapper;
    private readonly IBaseMapper<CourseDto, CourseModel> _courseModelMapper;

    public CourseMapperTest()
    {
        IMapper _mapper = MapperConfigurationFactory.CreateMapper();
        _courseDtoMapper = new BaseMapper<CourseModel, CourseDto>(_mapper);
        _courseModelMapper = new BaseMapper<CourseDto, CourseModel>(_mapper);
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
        dto.Teacher.Courses.Should().BeNull();
        dto.Students.Should().AllSatisfy(s => s.Courses.Should().BeNull());
    }

    [Theory]
    [MemberData(nameof(CourseTestData.CourseDtoTestCases), MemberType = typeof(CourseTestData))]
    public void Should_Map_Dto_To_Model(CourseDto dto)
    {
        var model = _courseModelMapper.MapInstance(dto);

        model.Id.Should().Be(dto.Id);
        model.Name.Should().Be(dto.Name);
        model.Credits.Should().Be(dto.Credits);
        model.TeacherId.Should().Be(dto.TeacherId);
    }
}

using FluentAssertions;
using AutoMapper;
using SchoolSystem.Core.Teacher;
using SchoolSystem.Core.Common.BaseInterfaces;
using SchoolSystem.Core.Common.BaseClasses;
using SchoolSystem.Core.Common;
using SchoolSystem.Core.Course;
using SchoolSystem.Tests.Core.Course;

namespace SchoolSystem.Tests.Core.Teacher;


public class TeacherMapperTest
{
    private readonly IBaseMapper<TeacherModel, TeacherDto> _courseDtoMapper;
    private readonly IBaseMapper<TeacherDto, TeacherModel> _courseModelMapper;

    public TeacherMapperTest()
    {
        IMapper _mapper = MapperConfigurationFactory.CreateMapper();
        _courseDtoMapper = new BaseMapper<TeacherModel, TeacherDto>(_mapper);
        _courseModelMapper = new BaseMapper<TeacherDto, TeacherModel>(_mapper);
    }

    [Theory]
    [MemberData(nameof(TeacherTestData.TeacherModelTestCases), MemberType = typeof(TeacherTestData))]
    public void Should_Map_Model_To_Dto(TeacherModel model)
    {
        List<CourseModel> courses = [CourseTestData.COURSE_MODEL_1_CALCULUS, CourseTestData.COURSE_MODEL_2_ALGEBRA];
        courses.ForEach(c => {
            c.Teacher = model;
            c.TeacherId = model.Id;
        });
        model.Courses = courses;

        var dto = _courseDtoMapper.MapInstance(model);

        dto.Id.Should().Be(model.Id);
        dto.FullName.Should().Be(model.FullName);
        dto.Email.Should().Be(model.Email);
        dto.Courses.Should().AllSatisfy(c => c.Teacher.Should().BeNull());
    }

    [Theory]
    [MemberData(nameof(TeacherTestData.TeacherDtoTestCases), MemberType = typeof(TeacherTestData))]
    public void Should_Map_Dto_To_Model(TeacherDto dto)
    {
        var model = _courseModelMapper.MapInstance(dto);

        model.Id.Should().Be(dto.Id);
        model.FullName.Should().Be(dto.FullName);
        model.Email.Should().Be(dto.Email);
    }
}

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
    private readonly IBaseMapper<TeacherModel, TeacherDetailsDto> _teacherDtoMapper;
    private readonly IBaseMapper<TeacherCreateDto, TeacherModel> _teacherCreateToModelMapper;
    private readonly IBaseMapper<TeacherUpdateDto, TeacherModel> _teacherUpdateToModelMapper;

    public TeacherMapperTest()
    {
        IMapper _mapper = MapperConfigurationFactory.CreateMapper();
        _teacherDtoMapper = new BaseMapper<TeacherModel, TeacherDetailsDto>(_mapper);
        _teacherCreateToModelMapper = new BaseMapper<TeacherCreateDto, TeacherModel>(_mapper);
        _teacherUpdateToModelMapper = new BaseMapper<TeacherUpdateDto, TeacherModel>(_mapper);
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

        var dto = _teacherDtoMapper.MapInstance(model);

        dto.Id.Should().Be(model.Id);
        dto.FullName.Should().Be(model.FullName);
        dto.Email.Should().Be(model.Email);
        dto.Courses.Should().AllSatisfy(c => c.Teacher.Should().BeNull());
    }

    [Theory]
    [MemberData(nameof(TeacherTestData.TeacherCreateDtoTestCases), MemberType = typeof(TeacherTestData))]
    public void Should_Map_Create_Dto_To_Model(TeacherCreateDto dto)
    {
        var model = _teacherCreateToModelMapper.MapInstance(dto);

        model.FullName.Should().Be(dto.FullName);
        model.Email.Should().Be(dto.Email);
    }

    [Theory]
    [MemberData(nameof(TeacherTestData.TeacherUpdateDtoTestCases), MemberType = typeof(TeacherTestData))]
    public void Should_Map_Update_Dto_To_Model(TeacherUpdateDto dto)
    {
        var model = _teacherUpdateToModelMapper.MapInstance(dto);

        model.Id.Should().Be(dto.Id);
        model.FullName.Should().Be(dto.FullName);
        model.Email.Should().Be(dto.Email);
    }
}

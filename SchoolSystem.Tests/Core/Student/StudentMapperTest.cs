using FluentAssertions;
using AutoMapper;
using SchoolSystem.Core.Student;
using SchoolSystem.Core.Common.BaseInterfaces;
using SchoolSystem.Core.Common.BaseClasses;
using SchoolSystem.Core.Common;
using SchoolSystem.Core.Teacher;
using SchoolSystem.Tests.Core.Teacher;
using SchoolSystem.Core.Course;
using SchoolSystem.Tests.Core.Course;
using System.Collections.Generic;

namespace SchoolSystem.Tests.Core.Student;


public class StudentMapperTest
{
    private readonly IBaseMapper<StudentModel, StudentDto> _studentDtoMapper;
    private readonly IBaseMapper<StudentDto, StudentModel> _studentModelMapper;

    public StudentMapperTest()
    {
        IMapper _mapper = MapperConfigurationFactory.CreateMapper();
        _studentDtoMapper = new BaseMapper<StudentModel, StudentDto>(_mapper);
        _studentModelMapper = new BaseMapper<StudentDto, StudentModel>(_mapper);
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
    [MemberData(nameof(StudentTestData.StudentDtoTestCases), MemberType = typeof(StudentTestData))]
    public void Should_Map_Dto_To_Model(StudentDto dto)
    {
        var model = _studentModelMapper.MapInstance(dto);

        model.Id.Should().Be(dto.Id);
        model.FullName.Should().Be(dto.FullName);
        model.Email.Should().Be(dto.Email);
    }
}

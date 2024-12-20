﻿using FluentAssertions;
using Moq;
using SchoolSystem.Core.Common.BaseInterfaces;
using SchoolSystem.Core.Course;
using SchoolSystem.Core.Student;
using SchoolSystem.Core.Teacher;
using SchoolSystem.Tests.Core.Student;
using SchoolSystem.Tests.Core.Teacher;

namespace SchoolSystem.Tests.Core.Course;


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


    public CourseServiceTests()
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

    [Fact]
    public async Task Should_Return_All_On_Get_All()
    {
        IEnumerable<CourseModel> coursesInDb = new List<CourseModel>
        {
                CourseTestData.COURSE_MODEL_2_ALGEBRA,
                CourseTestData.COURSE_MODEL_3_STATISTICS,
                CourseTestData.COURSE_MODEL_1_CALCULUS
        };
        List<CourseDto> coursesAsDto = new List<CourseDto>
        {
                CourseTestData.COURSE_DTO_2_ALGEBRA,
                CourseTestData.COURSE_DTO_3_STATISTICS,
                CourseTestData.COURSE_DTO_1_CALCULUS
        };

        _courseRepositoryMock.Setup(repo => repo.GetAll())
            .ReturnsAsync(coursesInDb);

        _courseDtoMapperMock.Setup(mapper => mapper.MapList(coursesInDb))
            .Returns(coursesAsDto);

        IEnumerable<CourseDto> coursesReturned = await courseService.GetAll();

        coursesReturned.Should().BeEquivalentTo(coursesAsDto);
    }

    [Fact]
    public async Task Should_Return_Correct_Data_On_GetSingle()
    {
        var courseModel = CourseTestData.COURSE_MODEL_1_CALCULUS;
        var courseAsDto = CourseTestData.COURSE_DTO_1_CALCULUS;
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
        var teacherModel = TeacherTestData.TEACHER_MODEL_2_ASSOCIATED;
        var teacherAsDto = TeacherTestData.TEACHER_DTO_2_ASSOCIATED;

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

        courseReturned.Should().Be(courseAsDto);
        courseReturned.Students.Should().BeEquivalentTo(studentsEnrolledAsDto);
        courseReturned.Teacher.Should().Be(teacherAsDto);
    }

    [Fact]
    public async Task Should_Return_Created_Data_On_Create()
    {
        var courseDto = CourseTestData.COURSE_DTO_3_STATISTICS;
        var courseAsModel = CourseTestData.COURSE_MODEL_3_STATISTICS;
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

        courseReturned.Name.Should().Be(courseDtoCreated.Name);
        courseReturned.Credits.Should().Be(courseDtoCreated.Credits);
        courseReturned.TeacherId.Should().Be(courseDtoCreated.TeacherId);
    }
}

using FluentAssertions;
using Xunit.Abstractions;
using Microsoft.Data.SqlClient;
using SchoolSystem.Core.Course;
using SchoolSystem.Core.Teacher;
using SchoolSystem.Infrastracture.Course;
using SchoolSystem.Infrastracture.Student;
using SchoolSystem.Infrastracture.Teacher;
using SchoolSystem.IntegrationTests.Common;
using SchoolSystem.IntegrationTests.Common.TestData;

namespace SchoolSystem.IntegrationTests.Infrastructure.Course;


public class CourseRepositoryIntegrationTest: IClassFixture<ContainerizedDatabaseFixture>
{
    private ContainerizedDatabaseFixture _fixture;
    private ITestOutputHelper _output;
    private CourseRepository _courseRepository;
    private StudentRepository _studentRepository;
    private TeacherRepository _teacherRepository;

    public CourseRepositoryIntegrationTest(ContainerizedDatabaseFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;

        _courseRepository = new CourseRepository(fixture.Context);
        _studentRepository = new StudentRepository(fixture.Context);
        _teacherRepository = new TeacherRepository(fixture.Context);   
    }

    [Fact]
    public async Task Should_Connection_Be_Available()
    {
        using var transactionalDatabaseRollback = new DatabaseRollbackTransactional(_fixture.Context);

        await using (var connection = new SqlConnection(_fixture.ConnectionString))
        {
            await connection.OpenAsync();
            connection.State.Should().Be(System.Data.ConnectionState.Open);
        }
    }

    [Fact]
    public async Task Should_Add_Entities_To_Repository()
    {
        using var transactionalDatabaseRollback = new DatabaseRollbackTransactional(_fixture.Context);

        TeacherModel createdTeacher = await _teacherRepository.Create(TeacherTestData.TEACHER_MODEL_1);
        CourseModel sampleCourse = CourseTestData.COURSE_MODEL_1; 
        sampleCourse.TeacherId = createdTeacher.Id;
        CourseModel createdCourse = await _courseRepository.Create(sampleCourse);

        createdCourse.Should().NotBeNull();
        createdCourse.Id.Should().NotBe(0);
    }

    [Fact]
    public async Task Should_Count_Zero_On_Empty_Repository()
    {
        using var transactionalDatabaseRollback = new DatabaseRollbackTransactional(_fixture.Context);

        int courseEntitiesCount = (await _courseRepository.GetAll()).Count();

        courseEntitiesCount.Should().Be(0);
    }

    [Fact]
    public async Task Should_Count_Increment_On_Create_Entity()
    {
        using var transactionalDatabaseRollback = new DatabaseRollbackTransactional(_fixture.Context);

        TeacherModel createdTeacher = await _teacherRepository.Create(TeacherTestData.TEACHER_MODEL_2);
        CourseModel sampleCourse = CourseTestData.COURSE_MODEL_2;
        sampleCourse.Teacher = createdTeacher;
        var _ = await _courseRepository.Create(sampleCourse);

        int courseEntitiesCount = (await _courseRepository.GetAll()).Count();

        courseEntitiesCount.Should().Be(1);
    }

    [Fact]
    public async Task Should_Count_Decrement_On_Delete_Entity()
    {
        using var transactionalDatabaseRollback = new DatabaseRollbackTransactional(_fixture.Context);

        TeacherModel createdTeacher = await _teacherRepository.Create(TeacherTestData.TEACHER_MODEL_2);
        CourseModel sampleCourse = CourseTestData.COURSE_MODEL_2;
        sampleCourse.TeacherId = createdTeacher.Id;
        CourseModel createdCourse = await _courseRepository.Create(sampleCourse);
        int courseEntitiesCount = (await _courseRepository.GetAll()).Count();

        await _courseRepository.Delete(sampleCourse);
        int courseEntitiesCountAfterDelete = (await _courseRepository.GetAll()).Count();

        courseEntitiesCountAfterDelete.Should().BeLessThan(courseEntitiesCount);
    }
}

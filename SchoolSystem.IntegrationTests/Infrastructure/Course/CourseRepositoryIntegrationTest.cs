using FluentAssertions;
using Microsoft.Data.SqlClient;
using SchoolSystem.Core.Course;
using SchoolSystem.Core.Teacher;
using SchoolSystem.Infrastracture.Course;
using SchoolSystem.Infrastracture.Student;
using SchoolSystem.Infrastracture.Teacher;
using SchoolSystem.IntegrationTests.Common;
using SchoolSystem.IntegrationTests.Common.TestData;
using SchoolSystem.Core.Student;
using SchoolSystem.IntegrationTests.Common.TestScenarios;

namespace SchoolSystem.IntegrationTests.Infrastructure.Course;

[Collection("RepositoryIntegrationTest")]
public class CourseRepositoryIntegrationTest
{
    private readonly ContainerizedDatabaseFixture _fixture;
    private readonly CourseRepository _courseRepository;
    private readonly StudentRepository _studentRepository;
    private readonly TeacherRepository _teacherRepository;

    public CourseRepositoryIntegrationTest(ContainerizedDatabaseFixture fixture)
    {
        _fixture = fixture;

        _courseRepository = new CourseRepository(fixture.Context);
        _studentRepository = new StudentRepository(fixture.Context);
        _teacherRepository = new TeacherRepository(fixture.Context);   
    }

    [Fact]
    public async Task Should_Connection_Be_Available()
    {
        await using (var connection = new SqlConnection(_fixture.ConnectionString))
        {
            await connection.OpenAsync();
            connection.State.Should().Be(System.Data.ConnectionState.Open);
        }
    }

    [Fact]
    public async Task Should_PreSeeded_Data_Be_Available()
    {
        using var seederCleaner = new DatabasePreSeederPostCleaner(_fixture.Context, CourseTestScenarios.SingleCourse);

        CourseModel? courseOnDatabase = (await _courseRepository.GetAll()).FirstOrDefault();
        StudentModel? studentOnDatabase = (await _studentRepository.GetAll()).FirstOrDefault();
        TeacherModel? teacherOnDatabase = (await _teacherRepository.GetAll()).FirstOrDefault();

        courseOnDatabase.Should().NotBeNull();
        studentOnDatabase.Should().NotBeNull();
        teacherOnDatabase.Should().NotBeNull();
        courseOnDatabase.Students.Should().Contain(studentOnDatabase);
        courseOnDatabase.Teacher.Should().Be(teacherOnDatabase);
        studentOnDatabase.Courses.Should().Contain(courseOnDatabase);
        teacherOnDatabase.Courses.Should().Contain(courseOnDatabase);
    }


    [Fact]
    public async Task Should_Add_Entities_To_Repository()
    {
        using var seederCleaner = new DatabasePreSeederPostCleaner(_fixture.Context, CourseTestScenarios.Empty);

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
        using var seederCleaner = new DatabasePreSeederPostCleaner(_fixture.Context, CourseTestScenarios.Empty);

        int courseEntitiesCount = (await _courseRepository.GetAll()).Count();

        courseEntitiesCount.Should().Be(0);
    }

    [Fact]
    public async Task Should_Count_Increment_On_Create_Entity()
    {
        using var seederCleaner = new DatabasePreSeederPostCleaner(_fixture.Context, CourseTestScenarios.SingleCourse);

        TeacherModel createdTeacher = await _teacherRepository.Create(TeacherTestData.TEACHER_MODEL_2);
        CourseModel sampleCourse = CourseTestData.COURSE_MODEL_2;
        sampleCourse.Teacher = createdTeacher;
        await _courseRepository.Create(sampleCourse);

        int courseEntitiesCount = (await _courseRepository.GetAll()).Count();

        courseEntitiesCount.Should().Be(2);
    }

    [Fact]
    public async Task Should_Count_Decrement_On_Delete_Entity()
    {
        using var seederCleaner = new DatabasePreSeederPostCleaner(_fixture.Context, CourseTestScenarios.SingleCourse);

        CourseModel? courseOnDatabase = (await _courseRepository.GetAll()).FirstOrDefault();
        if (courseOnDatabase != null)
        {
            await _courseRepository.Delete(courseOnDatabase);
        }

        int courseEntitiesCountAfterDelete = (await _courseRepository.GetAll()).Count();

        courseEntitiesCountAfterDelete.Should().BeLessThan(1);
    }
}

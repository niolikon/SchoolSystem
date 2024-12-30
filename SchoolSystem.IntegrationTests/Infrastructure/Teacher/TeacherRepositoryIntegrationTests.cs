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
using SchoolSystem.Core.Exceptions.Domain;

namespace SchoolSystem.IntegrationTests.Infrastructure.Teacher;

[Collection("RepositoryIntegrationTest")]
public class TeacherRepositoryIntegrationTests
{
    private readonly ContainerizedDatabaseFixture _fixture;
    private readonly CourseRepository _courseRepository;
    private readonly StudentRepository _studentRepository;
    private readonly TeacherRepository _teacherRepository;

    public TeacherRepositoryIntegrationTests(ContainerizedDatabaseFixture fixture)
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
        using var seederCleaner = new DatabasePreSeederPostCleaner(_fixture.Context, TeacherTestScenarios.SingleTeacher);

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
        using var seederCleaner = new DatabasePreSeederPostCleaner(_fixture.Context, TeacherTestScenarios.Empty);

        TeacherModel sampleTeacher = TeacherTestData.TEACHER_MODEL_2;
        TeacherModel? createdTeacher = await _teacherRepository.Create(sampleTeacher);

        createdTeacher.Should().NotBeNull();
        createdTeacher.Id.Should().NotBe(0);
    }

    [Fact]
    public async Task Should_Count_Zero_On_Empty_Repository()
    {
        using var seederCleaner = new DatabasePreSeederPostCleaner(_fixture.Context, TeacherTestScenarios.Empty);

        int teacherEntitiesCount = (await _teacherRepository.GetAll()).Count();

        teacherEntitiesCount.Should().Be(0);
    }

    [Fact]
    public async Task Should_Count_Increment_On_Create_Entity()
    {
        using var seederCleaner = new DatabasePreSeederPostCleaner(_fixture.Context, TeacherTestScenarios.SingleTeacher);

        TeacherModel teacherNotInScenario = TeacherTestData.TEACHER_MODEL_2;
        await _teacherRepository.Create(teacherNotInScenario);
        int teacherEntitiesCount = (await _teacherRepository.GetAll()).Count();

        teacherEntitiesCount.Should().Be(2);
    }

    [Fact]
    public async Task Should_Count_Decrement_On_Delete_Entity()
    {
        using var seederCleaner = new DatabasePreSeederPostCleaner(_fixture.Context, TeacherTestScenarios.SingleTeacher);

        TeacherModel? teacherInDatabase = (await _teacherRepository.GetAll()).FirstOrDefault();
        if (teacherInDatabase != null)
        {
            await _teacherRepository.Delete(teacherInDatabase);
        }
        int teacherEntitiesCountAfterDelete = (await _teacherRepository.GetAll()).Count();

        teacherEntitiesCountAfterDelete.Should().BeLessThan(1);
    }

    [Fact]
    public async Task Should_Not_Allow_Insert_With_Same_Email()
    {
        using var seederCleaner = new DatabasePreSeederPostCleaner(_fixture.Context, TeacherTestScenarios.SingleTeacher);

        TeacherModel teacherInScenario = TeacherTestData.TEACHER_MODEL_1;
        TeacherModel teacherNotInScenario = TeacherTestData.TEACHER_MODEL_2;
        teacherNotInScenario.Email = teacherInScenario.Email;

        Func<Task> insertTeacherWithDuplication = async () => await _teacherRepository.Create(teacherNotInScenario);
        await insertTeacherWithDuplication.Should()
            .ThrowAsync<EmailAlreadyExistsDomainException>();
    }

    [Fact]
    public async Task Should_Allow_Insert_With_Different_Email()
    {
        using var seederCleaner = new DatabasePreSeederPostCleaner(_fixture.Context, TeacherTestScenarios.SingleTeacher);

        TeacherModel teacherInScenario = TeacherTestData.TEACHER_MODEL_1;
        TeacherModel teacherNotInScenario = TeacherTestData.TEACHER_MODEL_2;
        teacherNotInScenario.Email = teacherInScenario.Email + ".dot";

        Func<Task> insertTeacherWithoutDuplication = async () => await _teacherRepository.Create(teacherNotInScenario);
        await insertTeacherWithoutDuplication.Should()
            .NotThrowAsync<EmailAlreadyExistsDomainException>();
    }
}

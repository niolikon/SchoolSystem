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

namespace SchoolSystem.IntegrationTests.Infrastructure.Student;

[Collection("RepositoryIntegrationTest")]
public class StudentRepositoryIntegrationTest
{
    private readonly ContainerizedDatabaseFixture _fixture;
    private readonly CourseRepository _courseRepository;
    private readonly StudentRepository _studentRepository;
    private readonly TeacherRepository _teacherRepository;

    public StudentRepositoryIntegrationTest(ContainerizedDatabaseFixture fixture)
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
        using var seederCleaner = new DatabasePreSeederPostCleaner(_fixture.Context, StudentTestScenarios.SingleStudent);

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
        using var seederCleaner = new DatabasePreSeederPostCleaner(_fixture.Context, StudentTestScenarios.Empty);

        StudentModel sampleStudent = StudentTestData.STUDENT_MODEL_2;
        StudentModel? createdStudent = await _studentRepository.Create(sampleStudent);

        createdStudent.Should().NotBeNull();
        createdStudent.Id.Should().NotBe(0);
    }

    [Fact]
    public async Task Should_Count_Zero_On_Empty_Repository()
    {
        using var seederCleaner = new DatabasePreSeederPostCleaner(_fixture.Context, StudentTestScenarios.Empty);

        int studentEntitiesCount = (await _studentRepository.GetAll()).Count();

        studentEntitiesCount.Should().Be(0);
    }

    [Fact]
    public async Task Should_Count_Increment_On_Create_Entity()
    {
        using var seederCleaner = new DatabasePreSeederPostCleaner(_fixture.Context, StudentTestScenarios.SingleStudent);

        StudentModel studentNotInScenario = StudentTestData.STUDENT_MODEL_2;
        await _studentRepository.Create(studentNotInScenario);
        int studentEntitiesCount = (await _studentRepository.GetAll()).Count();

        studentEntitiesCount.Should().Be(2);
    }

    [Fact]
    public async Task Should_Count_Decrement_On_Delete_Entity()
    {
        using var seederCleaner = new DatabasePreSeederPostCleaner(_fixture.Context, StudentTestScenarios.SingleStudent);

        StudentModel? studentInDatabase = (await _studentRepository.GetAll()).FirstOrDefault();
        if (studentInDatabase != null)
        {
            await _studentRepository.Delete(studentInDatabase);
        }
        int studentEntitiesCountAfterDelete = (await _studentRepository.GetAll()).Count();

        studentEntitiesCountAfterDelete.Should().BeLessThan(1);
    }

    [Fact]
    public async Task Should_Not_Allow_Insert_With_Same_Email()
    {
        using var seederCleaner = new DatabasePreSeederPostCleaner(_fixture.Context, StudentTestScenarios.SingleStudent);

        StudentModel studentInScenario = StudentTestData.STUDENT_MODEL_1;
        StudentModel studentNotInScenario = StudentTestData.STUDENT_MODEL_2;
        studentNotInScenario.Email = studentInScenario.Email;

        Func<Task> insertStudentWithDuplication = async () => await _studentRepository.Create(studentNotInScenario);
        await insertStudentWithDuplication.Should()
            .ThrowAsync<EmailAlreadyExistsDomainException>();
    }

    [Fact]
    public async Task Should_Allow_Insert_With_Different_Email()
    {
        using var seederCleaner = new DatabasePreSeederPostCleaner(_fixture.Context, StudentTestScenarios.SingleStudent);

        StudentModel studentInScenario = StudentTestData.STUDENT_MODEL_1;
        StudentModel studentNotInScenario = StudentTestData.STUDENT_MODEL_2;
        studentNotInScenario.Email = studentInScenario.Email + ".dot";

        Func<Task> insertStudentWithDuplication = async () => await _studentRepository.Create(studentNotInScenario);
        await insertStudentWithDuplication.Should()
            .NotThrowAsync<EmailAlreadyExistsDomainException>();
    }
}

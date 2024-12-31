using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using SchoolSystem.Core.Teacher;
using SchoolSystem.Infrastracture.Teacher;
using SchoolSystem.IntegrationTests.Common;
using SchoolSystem.IntegrationTests.Common.TestScenarios;

namespace SchoolSystem.IntegrationTests.Api.Course;

[Collection("ControllerIntegrationTest")]
public class TeacherControllerIntegrationTest
{
    private readonly ContainerizedDatabaseFixture _fixture;
    private readonly TeacherRepository _teacherRepository;

    public TeacherControllerIntegrationTest(ContainerizedDatabaseFixture fixture)
    {
        _fixture = fixture;
        _teacherRepository = new TeacherRepository(_fixture.Context);
    }

    [Fact]
    public async Task Get_All_Should_Return_No_Entities_On_Empty_Repository()
    {
        using var seederCleaner = new DatabasePreSeederPostCleaner(_fixture.Context, TeacherTestScenarios.Empty);

        HttpClient client = new ContainerizedWebApplicationFactory<Program>(_fixture).CreateClient();
        IEnumerable<TeacherDetailsDto>? returnedTeachers = await client.GetFromJsonAsync<IEnumerable<TeacherDetailsDto>>("/api/Teachers");

        returnedTeachers.Should().NotBeNull();
        returnedTeachers.Should().BeEmpty();
    }

    [Fact]
    public async Task Get_All_Should_Return_Entities_On_Not_Empty_Repository()
    {
        using var seederCleaner = new DatabasePreSeederPostCleaner(_fixture.Context, TeacherTestScenarios.SingleTeacher);

        HttpClient client = new ContainerizedWebApplicationFactory<Program>(_fixture).CreateClient();
        IEnumerable<TeacherDetailsDto>? returnedTeachers = await client.GetFromJsonAsync<IEnumerable<TeacherDetailsDto>>("/api/Teachers");

        returnedTeachers.Should().NotBeNull();
        returnedTeachers.Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task Post_Should_Insert_Teacher_With_Valid_Position()
    {
        using var seederCleaner = new DatabasePreSeederPostCleaner(_fixture.Context, TeacherTestScenarios.Empty);

        HttpClient client = new ContainerizedWebApplicationFactory<Program>(_fixture).CreateClient();
        TeacherCreateDto teacherToAdd = new()
            { Email = "teacher@email.com", FullName = "Mario Rossi", Position = AcademicPosition.PostdoctoralFellow.ToString() };
        HttpResponseMessage teacherPostResponse = await client.PostAsJsonAsync("/api/Teachers", teacherToAdd);
        teacherPostResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        teacherPostResponse.Content.Should().NotBeNull();
        TeacherDetailsDto? teacherCreated = await teacherPostResponse.Content.ReadFromJsonAsync<TeacherDetailsDto>();
        teacherCreated.Should().NotBeNull();
    }

    [Fact]
    public async Task Post_Should_Not_Insert_Teacher_With_InvValid_Position()
    {
        using var seederCleaner = new DatabasePreSeederPostCleaner(_fixture.Context, TeacherTestScenarios.Empty);

        HttpClient client = new ContainerizedWebApplicationFactory<Program>(_fixture).CreateClient();
        TeacherCreateDto teacherToAdd = new()
            { Email = "teacher@email.com", FullName = "Mario Rossi", Position = "NotValidPosition" };
        HttpResponseMessage teacherPostResponse = await client.PostAsJsonAsync("/api/Teachers", teacherToAdd);
        teacherPostResponse.StatusCode.Should().NotBe(HttpStatusCode.Created);
    }
}

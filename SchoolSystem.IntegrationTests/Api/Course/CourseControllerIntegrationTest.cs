using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using SchoolSystem.Core.Course;
using SchoolSystem.Core.Student;
using SchoolSystem.Core.Teacher;
using SchoolSystem.Infrastracture.Course;
using SchoolSystem.Infrastracture.Teacher;
using SchoolSystem.Infrastracture.Student;
using SchoolSystem.IntegrationTests.Common;
using SchoolSystem.IntegrationTests.Common.TestScenarios;

namespace SchoolSystem.IntegrationTests.Api.Course;

[Collection("ControllerIntegrationTest")]
public class CourseControllerIntegrationTest
{
    private readonly ContainerizedDatabaseFixture _fixture;
    private readonly StudentRepository _studentRepository;
    private readonly CourseRepository _courseRepository;
    private readonly TeacherRepository _teacherRepository;

    public CourseControllerIntegrationTest(ContainerizedDatabaseFixture fixture)
    {
        _fixture = fixture;
        _studentRepository = new StudentRepository(_fixture.Context);
        _courseRepository = new CourseRepository(_fixture.Context);
        _teacherRepository = new TeacherRepository(_fixture.Context);
    }

    [Fact]
    public async Task Get_All_Should_Return_No_Entities_On_Empty_Repository()
    {
        using var seederCleaner = new DatabasePreSeederPostCleaner(_fixture.Context, CourseTestScenarios.Empty);

        HttpClient client = new ContainerizedWebApplicationFactory<Program>(_fixture).CreateClient();
        IEnumerable<CourseDetailsDto>? returnedCourses = await client.GetFromJsonAsync<IEnumerable<CourseDetailsDto>>("/api/Courses");

        returnedCourses.Should().NotBeNull();
        returnedCourses.Should().BeEmpty();
    }

    [Fact]
    public async Task Get_All_Should_Return_Entities_On_Not_Empty_Repository()
    {
        using var seederCleaner = new DatabasePreSeederPostCleaner(_fixture.Context, CourseTestScenarios.SingleCourse);

        HttpClient client = new ContainerizedWebApplicationFactory<Program>(_fixture).CreateClient();
        IEnumerable<CourseDetailsDto>? returnedCourses = await client.GetFromJsonAsync<IEnumerable<CourseDetailsDto>>("/api/Courses");

        returnedCourses.Should().NotBeNull();
        returnedCourses.Should().NotBeEmpty();
    }
    
    
    [Fact]
    public async Task Controller_Should_Support_Enrollment_Workflow()
    {
        using var seederCleaner = new DatabasePreSeederPostCleaner(_fixture.Context, CourseTestScenarios.Empty);

        HttpClient client = new ContainerizedWebApplicationFactory<Program>(_fixture).CreateClient();
        TeacherCreateDto teacherToAdd = new()
            { Email = "teacher@email.com", FullName = "Mario Rossi", Position = AcademicPosition.PostdoctoralFellow.ToString() };
        HttpResponseMessage teacherPostResponse = await client.PostAsJsonAsync("/api/Teachers", teacherToAdd);
        teacherPostResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        teacherPostResponse.Content.Should().NotBeNull();
        TeacherDetailsDto? teacherCreated = await teacherPostResponse.Content.ReadFromJsonAsync<TeacherDetailsDto>();
        teacherCreated.Should().NotBeNull();
        
        CourseCreateDto courseToAdd = new() 
            { Credits = 10, Name = "Test", TeacherId = teacherCreated.Id };
        HttpResponseMessage coursePostResponse = await client.PostAsJsonAsync("/api/Courses", courseToAdd);
        coursePostResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        coursePostResponse.Content.Should().NotBeNull();
        CourseDetailsDto? courseCreated = await coursePostResponse.Content.ReadFromJsonAsync<CourseDetailsDto>();
        courseCreated.Should().NotBeNull();

        StudentCreateDto studentToAdd = new()
            { Email = "student@email.com", FullName = "Pino Pini" };
        HttpResponseMessage studentPostResponse = await client.PostAsJsonAsync("/api/Students", studentToAdd);
        studentPostResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        studentPostResponse.Content.Should().NotBeNull();
        StudentDetailsDto? studentCreated = await studentPostResponse.Content.ReadFromJsonAsync<StudentDetailsDto>();
        studentCreated.Should().NotBeNull();

        StudentUpdateDto studentToEnroll = new()
            { Id = studentCreated.Id, Email = studentCreated.Email, FullName = studentCreated.FullName };
        HttpResponseMessage courseEnrollmentPostResponse = await client.PostAsJsonAsync($"/api/Courses/{courseCreated.Id}/enrollments", studentToEnroll);
        courseEnrollmentPostResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        
        HttpResponseMessage courseUnEnrollmentPostResponse = await client.DeleteAsync($"/api/Courses/{courseCreated.Id}/enrollments/{studentToEnroll.Id}");
        courseUnEnrollmentPostResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}

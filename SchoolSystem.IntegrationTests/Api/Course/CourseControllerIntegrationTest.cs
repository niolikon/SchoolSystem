using System.Net;
using SchoolSystem.IntegrationTests.Common;
using System.Net.Http.Json;
using FluentAssertions;
using SchoolSystem.Core.Course;
using SchoolSystem.Core.Student;
using SchoolSystem.Core.Teacher;
using SchoolSystem.Infrastracture.Course;
using SchoolSystem.Infrastracture.Teacher;
using SchoolSystem.Infrastracture.Student;
using SchoolSystem.IntegrationTests.Infrastructure.Course;

namespace SchoolSystem.IntegrationTests.Api.Course;


public class CourseControllerIntegrationTest : IClassFixture<ContainerizedDatabaseFixture>
{
    private ContainerizedDatabaseFixture _fixture;
    private StudentRepository _studentRepository;
    private CourseRepository _courseRepository;
    private TeacherRepository _teacherRepository;

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
        using var seederCleaner = new DatabasePreSeederPostCleaner(_fixture.Context, CourseRepositoryScenarios.Empty);

        HttpClient client = new ContainerizedWebApplicationFactory<Program>(_fixture).CreateClient();
        IEnumerable<CourseDto> returnedCourses = await client.GetFromJsonAsync<IEnumerable<CourseDto>>("/api/Courses");

        returnedCourses.Should().BeEmpty();
    }

    [Fact]
    public async Task Get_All_Should_Return_Entities_On_Not_Empty_Repository()
    {
        using var seederCleaner = new DatabasePreSeederPostCleaner(_fixture.Context, CourseRepositoryScenarios.SingleCourse);

        HttpClient client = new ContainerizedWebApplicationFactory<Program>(_fixture).CreateClient();
        IEnumerable<CourseDto> returnedCourses = await client.GetFromJsonAsync<IEnumerable<CourseDto>>("/api/Courses");

        returnedCourses.Should().NotBeEmpty();
    }
    
    
    [Fact]
    public async Task Controller_Should_Support_Enrollment_Workflow()
    {
        using var seederCleaner = new DatabasePreSeederPostCleaner(_fixture.Context, CourseRepositoryScenarios.Empty);

        HttpClient client = new ContainerizedWebApplicationFactory<Program>(_fixture).CreateClient();
        TeacherDto teacherToAdd = new TeacherDto()
            { Email = "teacher@email.com", FullName = "Mario Rossi", Position = "Nice" };
        HttpResponseMessage teacherPostResponse = await client.PostAsJsonAsync("/api/Teachers", teacherToAdd);
        teacherPostResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        teacherPostResponse.Content.Should().NotBeNull();
        TeacherDto? teacherCreated = await teacherPostResponse.Content.ReadFromJsonAsync<TeacherDto>();
        teacherCreated.Should().NotBeNull();
        
        CourseDto courseToAdd = new CourseDto() 
            { Credits = 10, Name = "Test", TeacherId = teacherCreated.Id };
        HttpResponseMessage coursePostResponse = await client.PostAsJsonAsync("/api/Courses", courseToAdd);
        coursePostResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        coursePostResponse.Content.Should().NotBeNull();
        CourseDto? courseCreated = await coursePostResponse.Content.ReadFromJsonAsync<CourseDto>();
        courseCreated.Should().NotBeNull();

        StudentDto studentToAdd = new StudentDto()
            { Email = "student@email.com", FullName = "Pino Pini" };
        HttpResponseMessage studentPostResponse = await client.PostAsJsonAsync("/api/Students", studentToAdd);
        studentPostResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        studentPostResponse.Content.Should().NotBeNull();
        StudentDto? studentCreated = await studentPostResponse.Content.ReadFromJsonAsync<StudentDto>();
        studentCreated.Should().NotBeNull();

        HttpResponseMessage courseEnrollmentPostResponse = await client.PostAsJsonAsync($"/api/Courses/{courseCreated.Id}/enrollments", studentCreated);
        courseEnrollmentPostResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        
        HttpResponseMessage courseUnEnrollmentPostResponse = await client.DeleteAsync($"/api/Courses/{courseCreated.Id}/enrollments/{studentCreated.Id}");
        courseUnEnrollmentPostResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}

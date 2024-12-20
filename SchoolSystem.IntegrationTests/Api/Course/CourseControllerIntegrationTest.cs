using System.Net;
using SchoolSystem.IntegrationTests.Common;
using SchoolSystem.Core.Course;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using SchoolSystem.Core.Student;
using SchoolSystem.Core.Teacher;

namespace SchoolSystem.IntegrationTests.Api.Course;


public class CourseControllerIntegrationTest : IClassFixture<ContainerizedDatabaseFixture>
{
    private ContainerizedDatabaseFixture _fixture;

    public CourseControllerIntegrationTest(ContainerizedDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Get_Information_From_Database_Endpoint()
    {
        var client = new ContainerizedWebApplicationFactory<Program>(_fixture).CreateClient();
        var returnedCourses = await client.GetFromJsonAsync<IEnumerable<CourseDto>>("/api/Courses");
        returnedCourses.Should().BeEmpty();
    }
    
    
    [Fact]
    public async Task Add_Student_To_Course_On_Database_Endpoint()
    {
        using var transactionalDatabaseRollback = new DatabaseRollbackTransactional(_fixture.Context);
        
        var client = new ContainerizedWebApplicationFactory<Program>(_fixture).CreateClient();
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
        courseEnrollmentPostResponse.Content.Should().NotBeNull();
        CourseDto? courseWithEnrollment = await courseEnrollmentPostResponse.Content.ReadFromJsonAsync<CourseDto>();
        courseWithEnrollment.Should().NotBeNull();
        
        courseWithEnrollment.Students.Count().Should().Be(1);
    }
}

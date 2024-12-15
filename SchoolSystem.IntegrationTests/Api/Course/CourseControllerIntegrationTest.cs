using SchoolSystem.IntegrationTests.Common;
using SchoolSystem.Core.Course;
using System.Net.Http.Json;
using FluentAssertions;

namespace SchoolSystem.IntegrationTests.Api.Course;


public class CourseControllerIntegrationTest : IClassFixture<DatabaseContainerPerClassFixture>
{
    private DatabaseContainerPerClassFixture _fixture;

    public CourseControllerIntegrationTest(DatabaseContainerPerClassFixture fixture)
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
}

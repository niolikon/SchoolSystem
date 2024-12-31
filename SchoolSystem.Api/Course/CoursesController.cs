using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Core.Course;
using SchoolSystem.Core.Student;
using SchoolSystem.Core.Exceptions.Api;

namespace SchoolSystem.Api.Course;


[Route("api/[controller]")]
[ApiController]
public class CoursesController : ControllerBase
{
    private readonly ILogger<CoursesController>  _logger;
    private readonly ICourseService _courseService;

    public CoursesController(ILogger<CoursesController> logger, ICourseService courseService)
    {
        this._logger = logger;
        this._courseService = courseService;
    }

    // GET: api/<CourseController>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var courses = await _courseService.GetAll();
        return Ok(courses);
    }

    // GET: api/<CourseController>/paginated
    [HttpGet("paginated")]
    public async Task<IActionResult> GetAllPaginated(int? pageNumber, int? pageSize)
    {
        int pageSizeValue = (pageSize ?? 4);
        int pageNumberValue = (pageNumber ?? 1);

        var courses = await _courseService.GetAllPaginated(pageNumberValue, pageSizeValue);

        return Ok(courses);
    }

    // GET api/<CourseController>/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSingle(int id)
    {
        var course = await _courseService.GetSingle(id);
        return Ok(course);
    }

    // POST api/<CourseController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CourseCreateDto course)
    {
        if (!ModelState.IsValid)
        {
            throw new MalformedModelRestException("Course data is invalid");
        }

        var courseCreated = await _courseService.Create(course);
        return CreatedAtAction( 
            nameof(GetSingle),
            new { id = courseCreated.Id},
            courseCreated
        );
    }

    // PUT api/<CourseController>/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] CourseUpdateDto course)
    {
        if (!ModelState.IsValid)
        {
            throw new MalformedModelRestException("Course data is invalid");
        }

        await _courseService.Update(id, course);
        return Ok(course);
    }

    // DELETE api/<CourseController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _courseService.Delete(id);
        return NoContent();
    }

    // GET api/<CourseController>/5/enrollments
    [HttpGet("{id}/enrollments")]
    public async Task<IActionResult> GetAllEnrollments(int id)
    {
        IEnumerable<StudentDetailsDto> students = await _courseService.ListStudentsEnrolledToCourse(id);
        return Ok(students);

    }

    // POST api/<CourseController>/5/enrollments
    [HttpPost("{id}/enrollments")]
    public async Task<IActionResult> PostEnrollment(int id, [FromBody] StudentUpdateDto student)
    {
        if (!ModelState.IsValid)
        {
            throw new MalformedModelRestException("Course data is invalid");
        }
        
        await _courseService.AddStudentEnrollmentToCourse(id, student);
        return Created();
    }
    
    // DELETE api/<CourseController>/5/enrollments/20
    [HttpDelete("{id}/enrollments/{studentId}")]
    public async Task<IActionResult> DeleteEnrollment(int id, int studentId)
    {
        await _courseService.DeleteStudentEnrollmentToCourse(id, studentId);
        return NoContent();
    }
}

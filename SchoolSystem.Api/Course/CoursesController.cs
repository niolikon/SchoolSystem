using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Core.Course;
using SchoolSystem.Core.Student;

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
        try
        {
            var courses = await _courseService.GetAll();
            return Ok(courses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving courses");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // GET: api/<CourseController>/paginated
    [HttpGet("paginated")]
    public async Task<IActionResult> GetAllPaginated(int? pageNumber, int? pageSize)
    {
        try
        {
            int pageSizeValue = (pageSize ?? 4);
            int pageNumberValue = (pageNumber ?? 1);

            var courses = await _courseService.GetAllPaginated(pageNumberValue, pageSizeValue);

            return Ok(courses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving courses");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // GET api/<CourseController>/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSingle(int id)
    {
        try
        {
            var course = await _courseService.GetSingle(id);
            return Ok(course);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving courses");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // POST api/<CourseController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CourseDto course)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Course data is invalid");
        }

        try
        {
            var courseCreated = await _courseService.Create(course);
            return CreatedAtAction( 
                nameof(GetSingle),
                new { id = courseCreated.Id},
                courseCreated
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating course");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // PUT api/<CourseController>/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] CourseDto course)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Course data is invalid");
        }

        try
        {
            await _courseService.Update(id, course);
            return Ok(course);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating course");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // DELETE api/<CourseController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _courseService.Delete(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting course");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    
    // POST api/<CourseController>/5/enrollments
    [HttpPost("{id}/enrollments")]
    public async Task<IActionResult> PostEnrollment(int id, [FromBody] StudentDto student)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Student data is invalid");
        }

        try
        {
            await _courseService.EnrollStudentToCourse(student, id);
            return Created();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while enrolling student");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    
    // DELETE api/<CourseController>/5/enrollments
    [HttpDelete("{id}/enrollments")]
    public async Task<IActionResult> DeleteEnrollment(int id, [FromBody] StudentDto student)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Student data is invalid");
        }
        
        try
        {
            await _courseService.DisenrollStudentToCourse(student, id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while un-enrolling student");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}

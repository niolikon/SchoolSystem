using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Core.Course;

namespace SchoolSystem.Api.Course;


[Route("api/[controller]")]
[ApiController]
public class CoursesController : ControllerBase
{
    private readonly ILogger<CoursesController>  logger;
    private readonly ICourseService courseService;

    public CoursesController(ILogger<CoursesController> logger, ICourseService courseService)
    {
        this.logger = logger;
        this.courseService = courseService;
    }

    // GET: api/<CourseController>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var courses = await courseService.GetAll();
            return Ok(courses);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while retrieving courses");
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

            var courses = await courseService.GetAllPaginated(pageNumberValue, pageSizeValue);

            return Ok(courses);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while retrieving courses");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // GET api/<CourseController>/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSingle(int id)
    {
        try
        {
            var course = await courseService.GetSingle(id);
            return Ok(course);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while retrieving courses");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // POST api/<CourseController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CourseDto course)
    {
        if (course == null)
        {
            return BadRequest("Course data is null");
        }

        try
        {
            var courseCreated = await courseService.Create(course);
            return CreatedAtAction( 
                nameof(GetSingle),
                new { id = courseCreated.Id},
                courseCreated
            );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while creating course");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // PUT api/<CourseController>/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] CourseDto course)
    {
        if (course == null)
        {
            return BadRequest("Course data is null");
        }

        try
        {
            await courseService.Update(id, course);
            return Ok(course);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while updating course");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // DELETE api/<CourseController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await courseService.Delete(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while deleting course");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}

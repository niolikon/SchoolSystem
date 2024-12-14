using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Core.Course;
using SchoolSystem.Core.Student;

namespace SchoolSystem.Api.Student;


[Route("api/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    private readonly ILogger<StudentsController> logger;
    private readonly IStudentService studentService;

    public StudentsController(ILogger<StudentsController> logger, IStudentService studentService)
    {
        this.logger = logger;
        this.studentService = studentService;
    }

    // GET: api/<StudentController>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var students = await studentService.GetAll();
            return Ok(students);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while retrieving students");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // GET: api/<StudentController>/paginated
    [HttpGet("paginated")]
    public async Task<IActionResult> GetAllPaginated(int? pageNumber, int? pageSize)
    {
        try
        {
            int pageSizeValue = (pageSize ?? 4);
            int pageNumberValue = (pageNumber ?? 1);

            var students = await studentService.GetAllPaginated(pageNumberValue, pageSizeValue);

            return Ok(students);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while retrieving students");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // GET api/<StudentController>/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSingle(int id)
    {
        try
        {
            var student = await studentService.GetSingle(id);
            return Ok(student);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while retrieving students");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // POST api/<StudentController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] StudentDto student)
    {
        if (student == null)
        {
            return BadRequest("Student data is null");
        }

        try
        {
            var studentCreated = await studentService.Create(student);
            return CreatedAtAction(
                nameof(GetSingle),
                new { id = studentCreated.Id },
                studentCreated
            );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while creating student");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // PUT api/<StudentController>/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] StudentDto student)
    {
        if (student == null)
        {
            return BadRequest("Student data is null");
        }

        try
        {
            await studentService.Update(id, student);
            return Ok(student);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while updating student");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // DELETE api/<StudentController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await studentService.Delete(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while deleting student");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Core.Teacher;

namespace SchoolSystem.Api.Teacher;


[Route("api/[controller]")]
[ApiController]
public class TeachersController : ControllerBase
{
    private readonly ILogger<TeachersController> logger;
    private readonly ITeacherService teacherService;

    public TeachersController(ILogger<TeachersController> logger, ITeacherService teacherService)
    {
        this.logger = logger;
        this.teacherService = teacherService;
    }

    // GET: api/<TeacherController>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var teachers = await teacherService.GetAll();
            return Ok(teachers);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while retrieving teachers");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // GET: api/<TeacherController>/paginated
    [HttpGet("paginated")]
    public async Task<IActionResult> GetAllPaginated(int? pageNumber, int? pageSize)
    {
        try
        {
            int pageSizeValue = (pageSize ?? 4);
            int pageNumberValue = (pageNumber ?? 1);

            var teachers = await teacherService.GetAllPaginated(pageNumberValue, pageSizeValue);

            return Ok(teachers);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while retrieving teachers");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // GET api/<TeacherController>/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSingle(int id)
    {
        try
        {
            var teacher = await teacherService.GetSingle(id);
            return Ok(teacher);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while retrieving teachers");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // POST api/<TeacherController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] TeacherDto teacher)
    {
        if (teacher == null)
        {
            return BadRequest("Teacher data is null");
        }

        try
        {
            var teacherCreated = await teacherService.Create(teacher);
            return CreatedAtAction(
                nameof(GetSingle),
                new { id = teacherCreated.Id },
                teacherCreated
            );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while creating teacher");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // PUT api/<TeacherController>/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] TeacherDto teacher)
    {
        if (teacher == null)
        {
            return BadRequest("Teacher data is null");
        }

        try
        {
            await teacherService.Update(id, teacher);
            return Ok(teacher);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while updating teacher");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // DELETE api/<TeacherController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await teacherService.Delete(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while deleting teacher");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Core.Teacher;
using SchoolSystem.Core.Exceptions.Api;

namespace SchoolSystem.Api.Teacher;


[Route("api/[controller]")]
[ApiController]
public class TeachersController : ControllerBase
{
    private readonly ILogger<TeachersController> _logger;
    private readonly ITeacherService _teacherService;

    public TeachersController(ILogger<TeachersController> logger, ITeacherService teacherService)
    {
        this._logger = logger;
        this._teacherService = teacherService;
    }

    // GET: api/<TeacherController>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var teachers = await _teacherService.GetAll();
        return Ok(teachers);
    }

    // GET: api/<TeacherController>/paginated
    [HttpGet("paginated")]
    public async Task<IActionResult> GetAllPaginated(int? pageNumber, int? pageSize)
    {
        int pageSizeValue = (pageSize ?? 4);
        int pageNumberValue = (pageNumber ?? 1);

        var teachers = await _teacherService.GetAllPaginated(pageNumberValue, pageSizeValue);

        return Ok(teachers);
    }

    // GET api/<TeacherController>/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSingle(int id)
    {
        var teacher = await _teacherService.GetSingle(id);
        return Ok(teacher);
    }

    // POST api/<TeacherController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] TeacherDto teacher)
    {
        if (!ModelState.IsValid)
        {
            throw new MalformedModelRestException("Teacher data is invalid");
        }

        
        var teacherCreated = await _teacherService.Create(teacher);
        return CreatedAtAction(
            nameof(GetSingle),
            new { id = teacherCreated.Id },
            teacherCreated
        );
    }

    // PUT api/<TeacherController>/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] TeacherDto teacher)
    {
        if (!ModelState.IsValid)
        {
            throw new MalformedModelRestException("Teacher data is invalid");
        }

        await _teacherService.Update(id, teacher);
        return Ok(teacher);
    }

    // DELETE api/<TeacherController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _teacherService.Delete(id);
        return NoContent();
    }
}

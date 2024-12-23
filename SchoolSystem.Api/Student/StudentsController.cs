using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Core.Student;
using SchoolSystem.Core.Exceptions.Api;

namespace SchoolSystem.Api.Student;


[Route("api/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    private readonly ILogger<StudentsController> _logger;
    private readonly IStudentService _studentService;

    public StudentsController(ILogger<StudentsController> logger, IStudentService studentService)
    {
        this._logger = logger;
        this._studentService = studentService;
    }

    // GET: api/<StudentController>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var students = await _studentService.GetAll();
        return Ok(students);
    }

    // GET: api/<StudentController>/paginated
    [HttpGet("paginated")]
    public async Task<IActionResult> GetAllPaginated(int? pageNumber, int? pageSize)
    {
        int pageSizeValue = (pageSize ?? 4);
        int pageNumberValue = (pageNumber ?? 1);

        var students = await _studentService.GetAllPaginated(pageNumberValue, pageSizeValue);

        return Ok(students);
    }

    // GET api/<StudentController>/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSingle(int id)
    {
        var student = await _studentService.GetSingle(id);
        return Ok(student);
    }

    // POST api/<StudentController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] StudentDto student)
    {
        if (!ModelState.IsValid)
        {
            throw new MalformedModelRestException("Student data is invalid");
        }

        var studentCreated = await _studentService.Create(student);
        return CreatedAtAction(
            nameof(GetSingle),
            new { id = studentCreated.Id },
            studentCreated
        );
    }

    // PUT api/<StudentController>/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] StudentDto student)
    {
        if (!ModelState.IsValid)
        {
            throw new MalformedModelRestException("Student data is invalid");
        }
        
        await _studentService.Update(id, student);
        return Ok(student);
    }

    // DELETE api/<StudentController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _studentService.Delete(id);
        return NoContent();
    }
}

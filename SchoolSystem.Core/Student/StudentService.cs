using SchoolSystem.Core.Common.BaseInterfaces;
using SchoolSystem.Core.Common;
using SchoolSystem.Core.Exceptions.Api;
using SchoolSystem.Core.Exceptions.Domain;

namespace SchoolSystem.Core.Student;

public class StudentService(
    IBaseMapper<StudentModel, StudentDetailsDto> studentDtoMapper,
    IBaseMapper<StudentCreateDto, StudentModel> studentCreateToModelMapper,
    IBaseMapper<StudentUpdateDto, StudentModel> studentUpdateToModelMapper,
    IStudentRepository studentRepository) : IStudentService
{
    public async Task<IEnumerable<StudentDetailsDto>> GetAll()
    {
        try
        {
            return studentDtoMapper.MapList(await studentRepository.GetAllWithDetails());
        }
        catch (DatabaseOperationDomainException)
        {
            throw new InternalServerErrorRestException("An error prevents to retrieve all Students.");
        }
        catch (Exception)
        {
            throw new InternalServerErrorRestException("An error prevents to retrieve all Students.");
        }
    }

    public async Task<PaginatedData<StudentDetailsDto>> GetAllPaginated(int pageNumber, int pageSize)
    {
        try
        {
            PaginatedData<StudentModel> paginatedStudents = await studentRepository.GetPaginatedData(pageNumber, pageSize);
            List<StudentDetailsDto> students = studentDtoMapper.MapList(paginatedStudents.Data).ToList();

            return new PaginatedData<StudentDetailsDto>(students, paginatedStudents.TotalCount);
        }
        catch (InvalidQueryDomainException)
        {
            throw new RangeNotSatisfiableRestException($"Could not fetch the Students with the specified page {pageNumber} and size {pageSize}.");
        }
        catch (DatabaseOperationDomainException)
        {
            throw new InternalServerErrorRestException("An error prevents to retrieve Students paginated.");
        }
        catch (Exception)
        {
            throw new InternalServerErrorRestException("An error prevents to retrieve Students paginated.");
        }
    }

    public async Task<StudentDetailsDto> GetSingle(int id)
    {
        try
        {
            return studentDtoMapper.MapInstance(await studentRepository.GetByIdWithDetails(id));
        }
        catch (EntityNotFoundDomainException)
        {
            throw new NotFoundRestException($"Could not find Student with id {id}");
        }
        catch (DatabaseOperationDomainException)
        {
            throw new InternalServerErrorRestException($"An error prevents to retrieve Student with id {id}");
        }
        catch (Exception)
        {
            throw new InternalServerErrorRestException($"An error prevents to retrieve Student with id {id}.");
        }
    }

    public async Task<StudentDetailsDto> Create(StudentCreateDto dto)
    {
        try
        {
            StudentModel model = studentCreateToModelMapper.MapInstance(dto);
            model.EntryDate = DateTime.Now;

            return studentDtoMapper.MapInstance(await studentRepository.Create(model));
        }
        catch (EmailAlreadyExistsDomainException)
        {
            throw new ConflictRestException("This Student is trying to use an email already in use");
        }
        catch (EntityAlreadyExistsDomainException)
        {
            throw new ConflictRestException("Creating this Student violates an unique constraint");
        }
        catch (DatabaseOperationDomainException)
        {
            throw new InternalServerErrorRestException("An error prevents to create the Student.");
        }
        catch (Exception)
        {
            throw new InternalServerErrorRestException("An error prevents to create the Student.");
        }
    }

    public async Task Update(int id, StudentUpdateDto updateInputDto)
    {
        try
        {
            StudentModel existingStudent = await studentRepository.GetById(id);
            StudentModel updateInputStudent = studentUpdateToModelMapper.MapInstance(updateInputDto);

            existingStudent.FullName = updateInputStudent.FullName;
            existingStudent.Email = updateInputStudent.Email;
            existingStudent.UpdateDate = DateTime.Now;

            await studentRepository.Update(existingStudent);
        }
        catch (EmailAlreadyExistsDomainException)
        {
            throw new ConflictRestException("This Student is trying to use an email already in use");
        }
        catch (EntityNotFoundDomainException)
        {
            throw new NotFoundRestException($"Could not find Student with id {id}");
        }
        catch (DatabaseOperationDomainException)
        {
            throw new InternalServerErrorRestException("An error prevents to update the Student.");
        }
        catch (Exception)
        {
            throw new InternalServerErrorRestException("An error prevents to update the Student.");
        }
    }

    public async Task Delete(int id)
    {
        try
        {
            StudentModel student = await studentRepository.GetById(id);
            await studentRepository.Delete(student);
        }
        catch (EntityNotFoundDomainException)
        {
            throw new NotFoundRestException($"Could not find Student with id {id}");
        }
        catch (ForeignKeyViolationDomainException)
        {
            throw new ConflictRestException("Deleting this Student violates a foreign key constraint.");
        }
        catch (DatabaseOperationDomainException)
        {
            throw new InternalServerErrorRestException("An error prevents to delete the Student.");
        }
        catch (Exception)
        {
            throw new InternalServerErrorRestException("An error prevents to delete the Student.");
        }
    }
}

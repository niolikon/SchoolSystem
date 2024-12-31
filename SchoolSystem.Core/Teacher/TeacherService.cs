using SchoolSystem.Core.Common.BaseInterfaces;
using SchoolSystem.Core.Common;
using SchoolSystem.Core.Exceptions.Api;
using SchoolSystem.Core.Exceptions.Domain;

namespace SchoolSystem.Core.Teacher;

public class TeacherService(
    IBaseMapper<TeacherModel, TeacherDetailsDto> teacherDtoMapper,
    IBaseMapper<TeacherCreateDto, TeacherModel> teacherCreateToModelMapper,
    IBaseMapper<TeacherUpdateDto, TeacherModel> teacherUpdateToModelMapper,
    ITeacherRepository teacherRepository) : ITeacherService
{
    public async Task<IEnumerable<TeacherDetailsDto>> GetAll()
    {
        try
        {
            return teacherDtoMapper.MapList(await teacherRepository.GetAllWithDetails());
        }
        catch (DatabaseOperationDomainException)
        {
            throw new InternalServerErrorRestException("An error prevents to retrieve all Teachers.");
        }
        catch (Exception)
        {
            throw new InternalServerErrorRestException("An error prevents to retrieve all Teachers.");
        }
    }

    public async Task<PaginatedData<TeacherDetailsDto>> GetAllPaginated(int pageNumber, int pageSize)
    {
        try
        {
            PaginatedData<TeacherModel> paginatedTeachers = await teacherRepository.GetPaginatedData(pageNumber, pageSize);
            List<TeacherDetailsDto> teachers = teacherDtoMapper.MapList(paginatedTeachers.Data).ToList();

            return new PaginatedData<TeacherDetailsDto>(teachers, paginatedTeachers.TotalCount);
        }
        catch (InvalidQueryDomainException)
        {
            throw new RangeNotSatisfiableRestException($"Could not fetch the Teachers with the specified page {pageNumber} and size {pageSize}.");
        }
        catch (DatabaseOperationDomainException)
        {
            throw new InternalServerErrorRestException("An error prevents to retrieve Teachers paginated.");
        }
        catch (Exception)
        {
            throw new InternalServerErrorRestException("An error prevents to retrieve Teachers paginated.");
        }
    }

    public async Task<TeacherDetailsDto> GetSingle(int id)
    {
        try
        {
            return teacherDtoMapper.MapInstance(await teacherRepository.GetByIdWithDetails(id));
        }
        catch (EntityNotFoundDomainException)
        {
            throw new NotFoundRestException($"Could not find Teacher with id {id}");
        }
        catch (DatabaseOperationDomainException)
        {
            throw new InternalServerErrorRestException($"An error prevents to retrieve Teacher with id {id}");
        }
        catch (Exception)
        {
            throw new InternalServerErrorRestException($"An error prevents to retrieve Teacher with id {id}.");
        }
    }

    public async Task<TeacherDetailsDto> Create(TeacherCreateDto dto)
    {
        try
        {
            TeacherModel model = teacherCreateToModelMapper.MapInstance(dto);
            model.EntryDate = DateTime.Now;

            return teacherDtoMapper.MapInstance(await teacherRepository.Create(model));
        }
        catch (EmailAlreadyExistsDomainException)
        {
            throw new ConflictRestException("This Teacher is trying to use an email already in use");
        }
        catch (EntityAlreadyExistsDomainException)
        {
            throw new ConflictRestException("Creating this Teacher violates an unique constraint");
        }
        catch (DatabaseOperationDomainException)
        {
            throw new InternalServerErrorRestException("An error prevents to create the Teacher.");
        }
        catch (Exception)
        {
            throw new InternalServerErrorRestException("An error prevents to create the Teacher.");
        }
    }

    public async Task Update(int id, TeacherUpdateDto updateInputDto)
    {
        try
        {
            TeacherModel existingTeacher = await teacherRepository.GetById(id);
            TeacherModel updateInputTeacher = teacherUpdateToModelMapper.MapInstance(updateInputDto);

            existingTeacher.FullName = updateInputTeacher.FullName;
            existingTeacher.Position = updateInputTeacher.Position;
            existingTeacher.Email = updateInputTeacher.Email;
            existingTeacher.UpdateDate = DateTime.Now;

            await teacherRepository.Update(existingTeacher);
        }
        catch (EmailAlreadyExistsDomainException)
        {
            throw new ConflictRestException("This Teacher is trying to use an email already in use");
        }
        catch (EntityNotFoundDomainException)
        {
            throw new NotFoundRestException($"Could not find Teacher with id {id}");
        }
        catch (DatabaseOperationDomainException)
        {
            throw new InternalServerErrorRestException("An error prevents to update the Teacher.");
        }
        catch (Exception)
        {
            throw new InternalServerErrorRestException("An error prevents to update the Teacher.");
        }
    }

    public async Task Delete(int id)
    {
        try
        {
            TeacherModel teacher = await teacherRepository.GetById(id);
            await teacherRepository.Delete(teacher);
        }
        catch (EntityNotFoundDomainException)
        {
            throw new NotFoundRestException($"Could not find Teacher with id {id}");
        }
        catch (ForeignKeyViolationDomainException)
        {
            throw new ConflictRestException("Deleting this Teacher violates a foreign key constraint.");
        }
        catch (DatabaseOperationDomainException)
        {
            throw new InternalServerErrorRestException("An error prevents to delete the Teacher.");
        }
        catch (Exception)
        {
            throw new InternalServerErrorRestException("An error prevents to delete the Teacher.");
        }
    }
}

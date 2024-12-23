using SchoolSystem.Core.Common.BaseInterfaces;
using SchoolSystem.Core.Common;
using SchoolSystem.Core.Course;
using SchoolSystem.Core.Exceptions.Api;
using SchoolSystem.Core.Exceptions.Domain;


namespace SchoolSystem.Core.Teacher;

public class TeacherService(
    IBaseMapper<TeacherModel, TeacherDto> teacherDtoMapper,
    IBaseMapper<TeacherDto, TeacherModel> teacherModelMapper,
    ITeacherRepository teacherRepository,
    IBaseMapper<CourseModel, CourseDto> courseDtoMapper,
    ICourseRepository courseRepository) : ITeacherService
{
    public async Task<IEnumerable<TeacherDto>> GetAll()
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

    public async Task<PaginatedData<TeacherDto>> GetAllPaginated(int pageNumber, int pageSize)
    {
        try
        {
            PaginatedData<TeacherModel> paginatedTeachers = await teacherRepository.GetPaginatedData(pageNumber, pageSize);
            List<TeacherDto> teachers = teacherDtoMapper.MapList(paginatedTeachers.Data).ToList();

            return new PaginatedData<TeacherDto>(teachers, paginatedTeachers.TotalCount);
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

    public async Task<TeacherDto> GetSingle(int id)
    {
        try
        {
            return teacherDtoMapper.MapInstance(await teacherRepository.GetByIdWithDetails(id));
        }
        catch (EntityNotFoundDomainException e)
        {
            throw new NotFoundRestException($"Could not find Teacher with id {id}");
        }
        catch (DatabaseOperationDomainException e)
        {
            throw new InternalServerErrorRestException($"An error prevents to retrieve Teacher with id {id}");
        }
        catch (Exception)
        {
            throw new InternalServerErrorRestException($"An error prevents to retrieve Teacher with id {id}.");
        }
    }

    public async Task<TeacherDto> Create(TeacherDto dto)
    {
        try
        {
            TeacherModel model = teacherModelMapper.MapInstance(dto);
            model.EntryDate = DateTime.Now;

            return teacherDtoMapper.MapInstance(await teacherRepository.Create(model));
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

    public async Task Update(int id, TeacherDto updateInputDto)
    {
        try
        {
            TeacherModel existingTeacher = await teacherRepository.GetById(id);
            TeacherModel updateInputTeacher = teacherModelMapper.MapInstance(updateInputDto);

            existingTeacher.FullName = updateInputTeacher.FullName;
            existingTeacher.Position = updateInputTeacher.Position;
            existingTeacher.Email = updateInputTeacher.Email;
            existingTeacher.UpdateDate = DateTime.Now;

            await teacherRepository.Update(existingTeacher);
        }
        catch (EntityNotFoundDomainException e)
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
        catch (EntityNotFoundDomainException e)
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

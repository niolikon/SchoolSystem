using SchoolSystem.Core.Common.BaseInterfaces;
using SchoolSystem.Core.Common;
using SchoolSystem.Core.Student;
using SchoolSystem.Core.Teacher;
using SchoolSystem.Core.Exceptions.Api;
using SchoolSystem.Core.Exceptions.Domain;

namespace SchoolSystem.Core.Course;


public class CourseService(
    IBaseMapper<CourseModel, CourseDto> courseDtoMapper,
    IBaseMapper<CourseDto, CourseModel> courseModelMapper,
    ICourseRepository courseRepository,
    IBaseMapper<StudentModel, StudentDto> studentDtoMapper,
    IStudentRepository studentRepository,
    IBaseMapper<TeacherModel, TeacherDto> teacherDtoMapper,
    ITeacherRepository teacherRepository) : ICourseService
{
    public async Task<IEnumerable<CourseDto>> GetAll()
    {
        try
        {
            return courseDtoMapper.MapList(await courseRepository.GetAllWithDetails());
        }
        catch (DatabaseOperationDomainException)
        {
            throw new InternalServerErrorRestException("An error prevents to retrieve all Courses.");
        }
        catch (Exception)
        {
            throw new InternalServerErrorRestException("An error prevents to retrieve all Courses.");
        }
    }

    public async Task<PaginatedData<CourseDto>> GetAllPaginated(int pageNumber, int pageSize)
    {
        try
        {
            PaginatedData<CourseModel> paginatedCourses = await courseRepository.GetPaginatedData(pageNumber, pageSize);
            List<CourseDto> courses = courseDtoMapper.MapList(paginatedCourses.Data).ToList();

            return new PaginatedData<CourseDto>(courses, paginatedCourses.TotalCount);
        }
        catch (InvalidQueryDomainException)
        {
            throw new RangeNotSatisfiableRestException($"Could not fetch the Courses with the specified page {pageNumber} and size {pageSize}.");
        }
        catch (DatabaseOperationDomainException)
        {
            throw new InternalServerErrorRestException("An error prevents to retrieve Courses paginated.");
        }
        catch (Exception)
        {
            throw new InternalServerErrorRestException("An error prevents to retrieve Courses paginated.");
        }
    }

    public async Task<CourseDto> GetSingle(int id)
    {
        try
        {
            return courseDtoMapper.MapInstance(await courseRepository.GetByIdWithDetails(id));
        }
        catch(EntityNotFoundDomainException e)
        {
            throw new NotFoundRestException($"Could not find Course with id {id}");
        }
        catch (DatabaseOperationDomainException)
        {
            throw new InternalServerErrorRestException($"An error prevents to retrieve Course with id {id}.");
        }
        catch (Exception)
        {
            throw new InternalServerErrorRestException($"An error prevents to retrieve Course with id {id}.");
        }
    }

    public async Task<CourseDto> Create(CourseDto dto)
    {
        try
        {
            CourseModel model = courseModelMapper.MapInstance(dto);
            model.EntryDate = DateTime.Now;

            return courseDtoMapper.MapInstance(await courseRepository.Create(model));
        }
        catch (EntityAlreadyExistsDomainException)
        {
            throw new ConflictRestException("Creating this Course violates an unique constraint");
        }
        catch (DatabaseOperationDomainException)
        {
            throw new InternalServerErrorRestException("An error prevents to create the Course.");
        }
        catch (Exception)
        {
            throw new InternalServerErrorRestException("An error prevents to create the Course.");
        }
    }

    public async Task Update(int id, CourseDto updateInputDto)
    {
        try
        {
            CourseModel existingCourse = await courseRepository.GetById(id);
            CourseModel updateInputCourse = courseModelMapper.MapInstance(updateInputDto);

            existingCourse.Name = updateInputCourse.Name;
            existingCourse.Credits = updateInputCourse.Credits;
            existingCourse.UpdateDate = DateTime.Now;

            await courseRepository.Update(existingCourse);
        }
        catch (EntityNotFoundDomainException e)
        {
            throw new NotFoundRestException($"Could not find Course with id {id}");
        }
        catch (DatabaseOperationDomainException)
        {
            throw new InternalServerErrorRestException("An error prevents to update the Course.");
        }
        catch (Exception)
        {
            throw new InternalServerErrorRestException("An error prevents to update the Course.");
        }
    }

    public async Task Delete(int id)
    {
        try
        {
            CourseModel course = await courseRepository.GetById(id);
            await courseRepository.Delete(course);
        }
        catch (EntityNotFoundDomainException e)
        {
            throw new NotFoundRestException($"Could not find Course with id {id}");
        }
        catch (ForeignKeyViolationDomainException)
        {
            throw new ConflictRestException("Deleting this Course violates a foreign key constraint.");
        }
        catch (DatabaseOperationDomainException)
        {
            throw new InternalServerErrorRestException("An error prevents to delete the Course.");
        }
        catch (Exception)
        {
            throw new InternalServerErrorRestException("An error prevents to delete the Course.");
        }
    }

    public async Task<CourseDto> AddStudentEnrollmentToCourse(int courseId, StudentDto student)
    {
        try
        {
            CourseModel courseModel = await courseRepository.GetById(courseId);
            StudentModel studentModel = await studentRepository.GetById(student.Id);
            courseModel.Students.Add(studentModel);
            await courseRepository.Update(courseModel);
            return courseDtoMapper.MapInstance(courseModel);
        }
        catch (EntityNotFoundDomainException)
        {
            throw new NotFoundRestException("Could not find Course or Student.");
        }
        catch (DatabaseOperationDomainException)
        {
            throw new InternalServerErrorRestException("An error prevents to enroll the Student to the Course.");
        }
        catch (Exception)
        {
            throw new InternalServerErrorRestException("An error prevents to enroll the Student to the Course.");
        }
    }

    public async Task<CourseDto> DeleteStudentEnrollmentToCourse(int courseId, int studentId)
    {
        try
        {
            CourseModel courseModel = await courseRepository.GetById(courseId);
            StudentModel studentModel = await studentRepository.GetById(studentId);
            courseModel.Students.Remove(studentModel);
            await courseRepository.Update(courseModel);
            return courseDtoMapper.MapInstance(courseModel);
        }
        catch (EntityNotFoundDomainException)
        {
            throw new NotFoundRestException("Could not find Course or Student.");
        }
        catch (DatabaseOperationDomainException)
        {
            throw new InternalServerErrorRestException("An error prevents to un-enroll the Student from the Course.");
        }
        catch (Exception)
        {
            throw new InternalServerErrorRestException("An error prevents to un-enroll the Student from the Course.");
        }
    }

    public async Task<IEnumerable<StudentDto>> ListStudentsEnrolledToCourse(int courseId)
    {
        try
        {
            CourseDto courseDto = courseDtoMapper.MapInstance(await courseRepository.GetByIdWithDetails(courseId));
            return courseDto.Students;
        }
        catch (EntityNotFoundDomainException)
        {
            throw new NotFoundRestException("Could not find Course.");
        }
        catch (DatabaseOperationDomainException)
        {
            throw new InternalServerErrorRestException("An error prevents to retrieve the Students enrolled to the Course.");
        }
        catch (Exception)
        {
            throw new InternalServerErrorRestException("An error prevents to retrieve the Students enrolled to the Course.");
        }
    }
}

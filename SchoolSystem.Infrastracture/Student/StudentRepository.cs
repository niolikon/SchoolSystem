using SchoolSystem.Infrastracture.Common.BaseClasses;
using SchoolSystem.Infrastracture.Common;
using SchoolSystem.Core.Course;
using SchoolSystem.Core.Student;
using SchoolSystem.Core.Exceptions.Domain;
using Microsoft.EntityFrameworkCore;

namespace SchoolSystem.Infrastracture.Student;


public class StudentRepository : BaseRepository<StudentModel, int>, IStudentRepository
{
    public StudentRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<StudentModel>> FindStudentsByCourseId(int courseId)
    {
        IQueryable<StudentModel> students = _dbContext.Set<StudentModel>();
        IQueryable<CourseModel> courses = _dbContext.Set<CourseModel>();

        IQueryable<StudentModel> query =
            from s in students
            from c in courses
            where c.Id == courseId
            select s;

        try
        {
            return await query.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new DatabaseOperationDomainException("Failed to find Students by course ID.", ex);
        }
    }

    public override async Task<IEnumerable<StudentModel>> GetAllWithDetails()
    {
        IQueryable<StudentModel> students = _dbContext.Set<StudentModel>();

        IQueryable<StudentModel> query = (
            from s in students
            select new StudentModel()
            {
                Id = s.Id,
                FullName = s.FullName,
                Email = s.Email,
                Courses = (from c in s.Courses
                           select new CourseModel()
                           {
                               Id = c.Id,
                               Credits = c.Credits,
                               Name = c.Name
                           }).ToList()
            });

        try
        {
            return await query.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new DatabaseOperationDomainException("Failed to get Students with details.", ex);
        }
    }

    public override async Task<StudentModel> GetByIdWithDetails(int id)
    {
        IQueryable<StudentModel> students = _dbContext.Set<StudentModel>();

        IQueryable<StudentModel> query = (
            from s in students
            where s.Id == id
            select new StudentModel()
            {
                Id = s.Id,
                FullName = s.FullName,
                Email = s.Email,
                Courses = (from c in s.Courses
                           select new CourseModel()
                           {
                               Id = c.Id,
                               Credits = c.Credits,
                               Name = c.Name
                           }).ToList()
            });

        try
        {
            return await query.FirstOrDefaultAsync() ??
                throw new EntityNotFoundDomainException(typeof(StudentModel).ToString(), id);
        }
        catch (EntityNotFoundDomainException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new DatabaseOperationDomainException("Failed to retrieve Student details by ID.", ex);
        }
    }
}

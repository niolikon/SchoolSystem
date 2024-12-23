using SchoolSystem.Infrastracture.Common.BaseClasses;
using SchoolSystem.Infrastracture.Common;
using SchoolSystem.Core.Course;
using SchoolSystem.Core.Student;
using SchoolSystem.Core.Teacher;
using SchoolSystem.Core.Exceptions.Domain;
using Microsoft.EntityFrameworkCore;

namespace SchoolSystem.Infrastracture.Course;


public class CourseRepository : BaseRepository<CourseModel, int>, ICourseRepository
{
    public CourseRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<CourseModel>> FindCoursesByStudentId(int studentId)
    {
        IQueryable<CourseModel> courses = _dbContext.Set<CourseModel>();

        IQueryable<CourseModel> query =
            from c in courses
            from s in c.Students
            where s.Id == studentId
            select c;

        try
        {
            return await query.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new DatabaseOperationDomainException("Failed to find Courses by student ID.", ex);
        }
    }

    public async Task<IEnumerable<CourseModel>> FindCoursesByTeacherId(int teacherId)
    {
        IQueryable<CourseModel> courses = _dbContext.Set<CourseModel>();

        IQueryable<CourseModel> query =
            from c in courses
            where c.TeacherId == teacherId
            select c;

        try
        {
            return await query.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new DatabaseOperationDomainException("Failed to find Courses by teacher ID.", ex);
        }
    }

    public override async Task<IEnumerable<CourseModel>> GetAllWithDetails()
    {
        IQueryable<CourseModel> courses = _dbContext.Set<CourseModel>();

        IQueryable<CourseModel> query = (
            from c in courses
            select new CourseModel()
            {
                Credits = c.Credits,
                Id = c.Id,
                Name = c.Name,
                TeacherId = c.TeacherId,
                Teacher = (c.TeacherId == null) ? null : new TeacherModel()
                {
                    Id = c.Teacher.Id,
                    Email = c.Teacher.Email,
                    FullName = c.Teacher.FullName,
                    Position = c.Teacher.Position
                },
                Students = (from s in c.Students
                            select new StudentModel()
                            {
                                Id = s.Id,
                                Email = s.Email,
                                FullName = s.FullName
                            }).ToList()
            });

        try
        {
            return await query.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new DatabaseOperationDomainException("Failed to get Courses with details.", ex);
        }
    }

    public override async Task<CourseModel> GetByIdWithDetails(int id)
    {
        IQueryable<CourseModel> courses = _dbContext.Set<CourseModel>();

        IQueryable<CourseModel> query = (
            from c in courses
            where c.Id == id
            select new CourseModel()
            {
                Credits = c.Credits,
                Id = c.Id,
                Name = c.Name,
                TeacherId = c.TeacherId,
                Teacher = (c.TeacherId == null) ? null : new TeacherModel()
                {
                    Id = c.Teacher.Id,
                    Email = c.Teacher.Email,
                    FullName = c.Teacher.FullName,
                    Position = c.Teacher.Position
                },
                Students = (from s in c.Students
                            select new StudentModel()
                            {
                                Id = s.Id,
                                Email = s.Email,
                                FullName = s.FullName
                            }).ToList()
            });

        try
        {
            return await query.FirstOrDefaultAsync() ??
                    throw new EntityNotFoundDomainException(typeof(CourseModel).ToString(), id);
        }
        catch (EntityNotFoundDomainException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new DatabaseOperationDomainException("Failed to retrieve Course details by ID.", ex);
        }
    }
}
